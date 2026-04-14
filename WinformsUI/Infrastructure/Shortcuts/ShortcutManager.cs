using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace WinformsUI.Infrastructure.Shortcuts
{      
    /// <summary>
    /// Gestor de atajos globales para un Form. Captura a nivel de mensaje (IMessageFilter)
    /// para que funcionen combinaciones como Ctrl+Shift+C incluso con controles que consumen teclas.
    /// Ahora también soporta zoom con rueda (Ctrl + Wheel por defecto).
    /// </summary>
    public sealed class ShortcutManager : IDisposable, IMessageFilter
    {
        private readonly Form _form;
        private readonly Dictionary<Keys, Action> _handlers = new Dictionary<Keys, Action>();

        /// <summary>
        /// Atajos reservados cuando el foco está en controles de entrada (TextBox, ComboBox, DGV, etc.)
        /// para no interferir con comportamientos estándar de edición.
        /// </summary>
        private readonly HashSet<Keys> _reservedTextInput = new HashSet<Keys>
        {
            // Descomentar si querés respetar también estos atajos en inputs:
            // Keys.Control | Keys.C,
            // Keys.Control | Keys.A,
            Keys.Control | Keys.V,
            Keys.Control | Keys.X,
            Keys.Control | Keys.Z,
            Keys.Control | Keys.Y
        };

        // --- Wheel zoom bindings ---
        private Action _wheelUpHandler;
        private Action _wheelDownHandler;
        private Keys _wheelRequiredModifiers = Keys.Control;   // Ctrl + Wheel por defecto
        private int _wheelDeltaAccumulator = 0;                // acumula deltas (trackpad)

        private bool _disposed;

        private ShortcutManager(Form form)
        {
            _form = form ?? throw new ArgumentNullException(nameof(form));
            _form.KeyPreview = true;               // no molesta y ayuda en algunos casos
            Application.AddMessageFilter(this);     // captura a nivel de mensaje
        }

        /// <summary>Adjunta el gestor al formulario.</summary>
        public static ShortcutManager Attach(Form form) => new ShortcutManager(form);

        /// <summary>Agrega/overridea un atajo.</summary>
        public ShortcutManager Add(Keys keyData, Action handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            _handlers[keyData] = handler; // último gana
            return this;
        }

        /// <summary>Agrega/overridea un atajo a partir de un string (p.ej. "Ctrl+Shift+C").</summary>
        public ShortcutManager Add(string chord, Action handler) => Add(ParseChord(chord), handler);

        /// <summary>Agrega varios atajos de una.</summary>
        public ShortcutManager AddMany(params (string chord, Action handler)[] list)
        {
            foreach (var (chord, handler) in list) Add(chord, handler);
            return this;
        }

        public bool Remove(Keys keyData) => _handlers.Remove(keyData);
        public void Clear() => _handlers.Clear();

        /// <summary>
        /// Vincula zoom con rueda: por defecto Ctrl + Wheel (Up = ZoomIn, Down = ZoomOut).
        /// Cambiá requiredModifiers para usar Alt/Shift/etc.
        /// </summary>
        public ShortcutManager BindWheelZoom(Action onWheelUp, Action onWheelDown, Keys requiredModifiers = Keys.Control)
        {
            _wheelUpHandler = onWheelUp ?? throw new ArgumentNullException(nameof(onWheelUp));
            _wheelDownHandler = onWheelDown ?? throw new ArgumentNullException(nameof(onWheelDown));
            _wheelRequiredModifiers = requiredModifiers;
            return this;
        }

        /// <summary>Desvincula el zoom por rueda.</summary>
        public void UnbindWheelZoom()
        {
            _wheelUpHandler = null;
            _wheelDownHandler = null;
            _wheelDeltaAccumulator = 0;
        }



        private static bool IsDescendantOfOurForm(Control control, Form ourForm)
        {
            if (control == null || ourForm == null)
                return false;

            Control current = control;

            while (current != null)
            {
                if (current == ourForm)           // ¡Encontramos exactamente nuestro ClientForm!
                    return true;

                current = current.Parent;         // Subimos por .Parent (no .ParentForm)
            }
            return false;
        }


        /// <summary>
        /// Maneja mensajes antes de que los reciban los controles.
        /// Retorna true si el atajo fue manejado (consume el mensaje).
        /// </summary>
        public bool PreFilterMessage(ref Message m)
        {
            const int WM_KEYDOWN = 0x0100;
            const int WM_SYSKEYDOWN = 0x0104;
            const int WM_MOUSEWHEEL = 0x020A;
            const int WM_MOUSEHWHEEL = 0x020E; // no lo usamos para zoom, pero lo reconocemos

            // Teclado
            if (m.Msg == WM_KEYDOWN || m.Msg == WM_SYSKEYDOWN)
            {
                var target = Control.FromHandle(m.HWnd);
                if (target == null) return false;

                if (!IsDescendantOfOurForm(target, _form))
                    return false;

                var key = (Keys)((int)m.WParam) & Keys.KeyCode;
                var mods = Control.ModifierKeys; // Ctrl/Shift/Alt actuales
                var data = mods | key;

                // Si el foco está en controles de entrada y es un atajo reservado, no interceptamos
                if (IsTextInputControl(_form.ActiveControl) && _reservedTextInput.Contains(data))
                    return false;

                if (_handlers.TryGetValue(data, out var handler))
                {
                    try { handler?.Invoke(); }
                    catch (Exception ex) { Debug.WriteLine($"Shortcut error ({data}): {ex}"); }
                    return true; // consumir
                }

                return false;
            }

            // Rueda del mouse (zoom)
            if (m.Msg == WM_MOUSEWHEEL)
            {
                // ¿El mensaje pertenece a este form o a uno de sus hijos?
                var target = Control.FromHandle(m.HWnd);
                if (target == null) return false;
                var ownerForm = target.FindForm();
                if (ownerForm == null || ownerForm != _form)
                    return false;

                if (_wheelUpHandler == null || _wheelDownHandler == null)
                    return false;

                // Verificar modificadores requeridos (por defecto Ctrl)
                if ((_wheelRequiredModifiers != Keys.None) &&
                    ((Control.ModifierKeys & _wheelRequiredModifiers) != _wheelRequiredModifiers))
                    return false;

                // WHEEL_DELTA = 120 por "notch"
                // delta = HIWORD(wParam) con signo
                int wParam = m.WParam.ToInt32();
                int delta = (short)((wParam >> 16) & 0xFFFF);
                if (delta == 0) return false;

                _wheelDeltaAccumulator += delta;

                const int WHEEL_DELTA = 120;
                while (Math.Abs(_wheelDeltaAccumulator) >= WHEEL_DELTA)
                {
                    if (_wheelDeltaAccumulator > 0)
                    {
                        // rueda hacia arriba
                        try { _wheelUpHandler?.Invoke(); }
                        catch (Exception ex) { Debug.WriteLine($"WheelUp error: {ex}"); }
                        _wheelDeltaAccumulator -= WHEEL_DELTA;
                    }
                    else
                    {
                        // rueda hacia abajo
                        try { _wheelDownHandler?.Invoke(); }
                        catch (Exception ex) { Debug.WriteLine($"WheelDown error: {ex}"); }
                        _wheelDeltaAccumulator += WHEEL_DELTA;
                    }
                }

                return true; // consumimos para que no scrollee si estamos en modo zoom
            }

            // No usamos horizontal wheel para zoom; lo dejamos pasar
            if (m.Msg == WM_MOUSEHWHEEL)
                return false;

            return false;
        }

        /// <summary>Intenta manejar un atajo directamente (útil si preferís llamar desde ProcessCmdKey).</summary>
        public bool TryHandle(Keys data)
        {
            if (IsTextInputControl(_form.ActiveControl) && _reservedTextInput.Contains(data))
                return false;

            if (_handlers.TryGetValue(data, out var handler))
            {
                handler?.Invoke();
                return true;
            }
            return false;
        }

        private static bool IsTextInputControl(Control c)
        {
            if (c == null) return false;
            return c is TextBoxBase || c is ComboBox || c is DataGridView || c is RichTextBox;
        }

        /// <summary>
        /// Parsea acordes como "Ctrl+S", "Ctrl+Shift+N", "Alt+Enter", "F5", "Esc",
        /// "Ctrl+Delete", "Alt+Left", "Ctrl+Plus" / "Ctrl+Minus".
        /// </summary>
        public static Keys ParseChord(string chord)
        {
            if (string.IsNullOrWhiteSpace(chord))
                throw new ArgumentException("Chord vacío.", nameof(chord));

            var parts = chord.Split(new[] { '+', '-', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                             .Select(p => p.Trim().ToLowerInvariant()).ToList();

            Keys mods = Keys.None;
            Keys key = Keys.None;

            foreach (var p in parts)
            {
                switch (p)
                {
                    case "ctrl":
                    case "control": mods |= Keys.Control; break;
                    case "shift": mods |= Keys.Shift; break;
                    case "alt": mods |= Keys.Alt; break;

                    case "esc":
                    case "escape": key = Keys.Escape; break;
                    case "enter": key = Keys.Enter; break;
                    case "space":
                    case "spacebar": key = Keys.Space; break;
                    case "tab": key = Keys.Tab; break;
                    case "delete":
                    case "del": key = Keys.Delete; break;
                    case "back":
                    case "backspace": key = Keys.Back; break;

                    case "left": key = Keys.Left; break;
                    case "right": key = Keys.Right; break;
                    case "up": key = Keys.Up; break;
                    case "down": key = Keys.Down; break;

                    case "plus":
                    case "+": key = Keys.Add; break; // Numpad '+'
                    case "minus":
                    case "-": key = Keys.Subtract; break; // Numpad '-'
                    case "multiply":
                    case "*": key = Keys.Multiply; break;
                    case "divide":
                    case "/": key = Keys.Divide; break;
                    case "decimal":
                    case ".": key = Keys.Decimal; break;

                    case "home": key = Keys.Home; break;
                    case "end": key = Keys.End; break;
                    case "pageup":
                    case "pgup": key = Keys.PageUp; break;
                    case "pagedown":
                    case "pgdn": key = Keys.PageDown; break;
                    case "insert":
                    case "ins": key = Keys.Insert; break;

                    default:
                        if (p.StartsWith("f") && int.TryParse(p.Substring(1), out var fn) && fn >= 1 && fn <= 24)
                        {
                            key = Keys.F1 + (fn - 1);
                        }
                        else if (p.Length == 1)
                        {
                            var ch = char.ToUpperInvariant(p[0]);
                            if (ch >= 'A' && ch <= 'Z') key = (Keys)ch;
                            else if (ch >= '0' && ch <= '9') key = (Keys)ch;
                            else throw new ArgumentException($"Tecla no reconocida: '{p}'");
                        }
                        else
                        {
                            throw new ArgumentException($"Parte no reconocida: '{p}'");
                        }
                        break;
                }
            }

            if (key == Keys.None)
                throw new ArgumentException($"Chord inválido, falta tecla principal: '{chord}'");

            return mods | key;
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            Application.RemoveMessageFilter(this);
        }
    }
}
