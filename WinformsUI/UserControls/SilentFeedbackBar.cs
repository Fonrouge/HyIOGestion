using SharedAbstractions.Enums;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;
using Winforms.Theme;

namespace WinformsUI.UserControls
{


    public class SilentFeedbackBar : Control
    {
        private Timer animationTimer;
        private float intensity = 0f; // 0 = Gris, 1 = Color pleno
        private float shiftOffset = 0f; // Para el efecto loading
        private FeedbackState currentState = FeedbackState.Idle;
        private bool fadingOut = false;

        private int holdCounter = 0;
        private const int MaxHoldTicks = 15; // Aproximadamente 450ms de pausa en la cima

        // Colores base (Estilo Google/Multicolor)
        private Color[] baseColorsIAMode = { Color.RoyalBlue, Color.Crimson, Color.Gold, Color.SeaGreen };

        private Color[] baseColorsNormalMode = { Color.FromArgb(14, 80, 228), Color.FromArgb(113, 64, 225), Color.FromArgb(13, 184, 233), Color.FromArgb(14, 80, 228) };

        private float[] colorPositions = { 0.0f, 0.33f, 0.66f, 1.0f };
        private TaskCompletionSource<bool> _animationTaskSource;


        private DarkTheme.Palette _palette = DarkTheme.GetCurrentPalette();
        public bool IsActiveModule { get; set; } = true;

        public SilentFeedbackBar()
        {
            this.DoubleBuffered = true;
            this.Height = 4;
            this.Dock = DockStyle.Top;

            animationTimer = new Timer { Interval = 20 }; // Un pelín más lento para que sea sutil
            animationTimer.Tick += AnimationTimer_Tick;
            animationTimer.Start(); // Arranca siempre
        }
        public void TriggerFeedback(FeedbackState state)
        {
            currentState = state;
            intensity = 0f;
            fadingOut = false;
            shiftOffset = 0f;
            holdCounter = 0; // Reiniciamos el contador cada vez
            animationTimer.Start();
        }

        public void ActiveModule(bool isActive)
        {
            IsActiveModule = isActive;
        }

        public async Task<bool> TriggerFeedbackAsync(FeedbackState state)
        {
            // Si ya hay una animación corriendo, la cancelamos o terminamos
            _animationTaskSource?.TrySetResult(false);
            _animationTaskSource = new TaskCompletionSource<bool>();

            // Seteamos el estado (esto dispara la lógica que ya tenés en el Timer)
            TriggerFeedback(state);

            // Si es Loading, devolvemos true inmediatamente porque no tiene "fin" automático
            if (state == FeedbackState.Loading) return true;

            // Esperamos a que el Timer nos avise que terminó
            return await _animationTaskSource.Task;
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            // El offset del flujo constante (Idle/Loading)
            float speed = (currentState == FeedbackState.Loading) ? 0.04f : 0.01f;
            shiftOffset += speed;
            if (shiftOffset > 1f) shiftOffset = 0f;

            if (currentState == FeedbackState.Success || currentState == FeedbackState.Error)
            {
                if (!fadingOut)
                {
                    intensity += 0.20f; // Ataque rápido

                    if (intensity >= 1f)
                    {
                        intensity = 1f;

                        // Mantenemos la recompensa visual antes de empezar a decaer
                        holdCounter++;
                        if (holdCounter >= MaxHoldTicks)
                        {
                            fadingOut = true;
                            holdCounter = 0;
                        }
                    }
                }
                else
                {
                    intensity -= 0.05f; // Recomposición ágil

                    if (intensity <= 0f)
                    {
                        intensity = 0f;
                        fadingOut = false;
                        currentState = FeedbackState.Idle;

                        // NOTIFICACIÓN DE FIN:
                        _animationTaskSource?.TrySetResult(true);
                    }
                }
            }
            this.Invalidate();
        }



        public void ChangePalette(DarkTheme.Palette p) => _palette = p;


        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color[] targetColors = new Color[baseColorsNormalMode.Length];

            Color appBackgroundColor = Color.FromArgb(30, 30, 30); // O el color exacto que tengas



            Color[] currentPaletteColors;

            // 2. Evaluamos y asignamos con la sintaxis correcta de arreglos
            if (DarkTheme.IsDarkPalette(_palette))
            {
                // Aplicamos el 20% de oscuridad a cada componente del degradado
                currentPaletteColors = new Color[]
                {
                    DarkTheme.Darken(_palette.Accent, 0.9),  //Color.Red,
                    DarkTheme.Darken(_palette.Accent, 0),  //Color.Green,
                    DarkTheme.Darken(_palette.Accent, 0.9),  //Color.Red,
                    DarkTheme.Darken(_palette.Accent, 0.9)
                };
            }
            else
            {
                // En modo claro usamos los acentos puros para que resalten sobre el blanco
                currentPaletteColors = new Color[]
                {
                    DarkTheme.Darken(_palette.Accent, -0.9),  //Color.Green,
                    DarkTheme.Darken(_palette.Accent, 0),  //Color.Red,
                    DarkTheme.Darken(_palette.Accent, -0.9),  //Color.Red,
                    DarkTheme.Darken(_palette.Accent, -0.9)
                };
            }

            for (int i = 0; i < baseColorsIAMode.Length; i++)
            {
                if (currentState == FeedbackState.Success) // ÉXITO (Manda sobre el foco): Verde intenso
                    targetColors[i] = BlendColor(appBackgroundColor, Color.LimeGreen, intensity);

                else if (currentState == FeedbackState.Error) // ERROR (Manda sobre el foco): Rojo intenso                    
                    targetColors[i] = BlendColor(appBackgroundColor, Color.Red, intensity);

                else if (currentState == FeedbackState.Loading) // LOADING: Multicolor pleno (para indicar proceso activo)
                    targetColors[i] = baseColorsNormalMode[i];

                else // ESTADO IDLE
                {
                    if (IsActiveModule) // ACTIVO: Multicolor (saturación media/alta)
                        targetColors[i] = BlendColor(currentPaletteColors[i], _palette.Accent, 0.15f); // 70% gris oscuro -- ACA DETERMINO CUAN OSCURO EL IDLE
                    else
                        // INACTIVO: Escala de grises (Desaturación total)
                        targetColors[i] = BlendColor(currentPaletteColors[i], Color.Black, 0.7f); // 70% gris oscuro -- ACA DETERMINO CUAN OSCURO EL IDLE
                }
            }

            ColorBlend blend = new ColorBlend();
            blend.Colors = targetColors;
            blend.Positions = new float[] { 0.0f, 0.33f, 0.66f, 1.0f };

            // Definimos el ancho del gradiente. 
            // Para que el movimiento sea fluido, el rect del brush debe ser igual al control.
            Rectangle brushRect = new Rectangle(0, 0, this.Width, this.Height);

            using (LinearGradientBrush brush = new LinearGradientBrush(brushRect, Color.Black, Color.Black, 0f))
            {
                brush.InterpolationColors = blend;
                brush.WrapMode = WrapMode.Tile; // Crucial para el efecto infinito

                // Aplicamos el movimiento tanto en Loading como en Idle
                if (currentState == FeedbackState.Loading || currentState == FeedbackState.Idle)
                {
                    Matrix matrix = new Matrix();
                    matrix.Translate(shiftOffset * this.Width, 0);
                    brush.MultiplyTransform(matrix);
                }

                g.FillRectangle(brush, this.ClientRectangle);
            }
        }
        // Función auxiliar para mezclar colores (Interpolación lineal)
        private Color BlendColor(Color backColor, Color color, float amount)
        {
            byte r = (byte)((backColor.R * (1 - amount)) + (color.R * amount));
            byte g = (byte)((backColor.G * (1 - amount)) + (color.G * amount));
            byte b = (byte)((backColor.B * (1 - amount)) + (color.B * amount));
            return Color.FromArgb(r, g, b);
        }

    }
}