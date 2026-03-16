===========================================================================
                            LAYER: DOMAIN (CORE)
===========================================================================

ESTRATEGIA ARQUITECTÓNICA:
--------------------------
Organización: Vertical Slicing (Segmentación Vertical). 
Justificación: Se agrupa por Agregado/Entidad con todas sus interfaces y objetos 
relacionados (Value Objects, Repositories). Facilita enormemente el buscado
y encontrado de una clase y sus relacionados directos. 

PATRONES DE DISEÑO IMPLEMENTADOS:
---------------------------------
1. Rich Domain Model: Las entidades contienen lógica y estado, evitando modelos 
   anémicos/primitvos. La entidad es la única dueña de sus procesos de negocio.
2. Value Objects (VO): Tipado fuerte e inmutabilidad para datos descriptivos. 
   Elimina la "Obsesión por Primitivos".
3. Repository Pattern (Interfaces): Definición de contratos de persistencia 
   dentro del dominio para cumplir con la Inversión de Dependencias (DIP).
4. Layer Supertype: Uso de clases base (EntityBase, IValueObject) para centralizar 
   comportamientos transversales como identidad e igualdad.
5. Specification: Encapsulamiento de reglas de consulta y validación complejas, 
   haciéndolas reutilizables y testables.
6. Unit of Work (Interface): Abstracción para garantizar la atomicidad y 
   consistencia de las transacciones entre múltiples agregados.

PROBLEMAS RESUELTOS (JUSTIFICACIÓN TÉCNICA):
--------------------------------------------
1. Defensa en profundidad (Secure by Design): Implementación de entidades 
   "Always-Valid". Mediante constructores privados y métodos de fábrica, es 
   imposible instanciar objetos en un estado inconsistente o inválido.
2. Encapsulamiento de Validación: Cada campo (VO) es responsable de su propia 
   integridad. El dominio no pregunta si un dato es válido, el dato garantiza 
   su validez por existencia.
3. Desacoplamiento de Persistencia: La DAL (Data Access Layer) es agnóstica a 
   las reglas de negocio. La entidad dicta qué operaciones son permitidas, 
   protegiendo el modelo de manipulaciones arbitrarias desde la infraestructura.
4. Extensibilidad y Abstracción: El diseño admite el uso de genéricos para 
   operaciones base, facilitando la implementación de repositorios genéricos 
   sin perder el tipado fuerte.
5. Comportamientos Polimórficos (BaseContracts): Mediante interfaces como 
   ISoftDeletable, se definen capacidades que la infraestructura puede ejecutar 
   automáticamente (ej. filtrado de bajas lógicas) sin intervención de la BLL.
6. Integridad Transaccional: Se eliminan las inconsistencias en base de datos 
   al asegurar que todos los cambios de una operación de negocio se confirmen 
   o reviertan en bloque.

NOTA / ACLARACIONES:
--------------------------------------------
* Influencia Arquitectónica: Inspirado fuertemente en Clean Architecture, con 
  un Domain dueño absoluto de sus propias reglas, invariantes y acciones.
* Adaptación de Cátedra: Los "Casos de Uso" (Application Core) fueron delegados 
  a la capa BLL para respetar estrictamente el requisito académico de 
  Arquitectura N-Layer, manteniendo la lógica de orquestación separada.
* Enfoque Hexagonal: Se implementa el concepto de "Ports" (Interfaces) para que 
  las capas superiores consuman el dominio sin acoplamiento concreto.
* Paradigma: DDD (Domain-Driven Design) + IoC (Inversion of Control).

===========================================================================