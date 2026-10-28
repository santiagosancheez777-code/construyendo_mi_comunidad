namespace ConstruyendoMiComunidad
{
    /// <summary>
    /// Textos educativos y de juego en español. Centralizados para facilitar cambios.
    /// </summary>
    public static class GameContent
    {
        public const string Titulo = "Construyendo mi comunidad";
        public const string Subtitulo = "La importancia de la participación comunitaria";

        public const string Instrucciones =
            "Controles\n\n" +
            "• WASD o flechas: mover al personaje\n" +
            "• Ratón: girar la cámara\n" +
            "• Mayús: correr\n" +
            "• E: hablar, recoger residuos o usar un espacio\n" +
            "• Esc: pausar\n\n" +
            "Objetivo\n\n" +
            "Recorre el barrio, escucha a tus vecinas y vecinos, participa en las " +
            "actividades comunitarias y toma decisiones colectivas. Cada acción demuestra " +
            "que la colaboración permite soluciones más justas y sostenibles.\n\n" +
            "Puntos\n\n" +
            "• Hablar con un habitante: 10\n" +
            "• Completar una actividad comunitaria: 20\n" +
            "• Responder bien una pregunta: 10\n" +
            "• Completar una misión: 30";

        public static readonly string[] MisionTitulos =
        {
            "Misión 1: Conoce tu comunidad",
            "Misión 2: Recuperemos el parque",
            "Misión 3: Asamblea de vecinos",
            "Misión 4: Construyamos juntos",
            "Misión 5: El caso de Medellín",
            "Misión 6: Pon a prueba tus conocimientos"
        };

        public static readonly string[] MisionObjetivos =
        {
            "Habla con al menos 3 habitantes del barrio (E).",
            "Recoge los residuos y deposítalos en la caneca del color correcto.",
            "Entra al centro comunitario y elige soluciones con la asamblea.",
            "Repara 3 zonas deterioradas colaborando con la comunidad.",
            "Relaciona problemas del barrio con soluciones construidas en conjunto.",
            "Responde el cuestionario sobre participación comunitaria."
        };

        public static readonly string[] MensajesEducativos =
        {
            "La participación comunitaria es el proceso mediante el cual las personas " +
            "se involucran de forma activa y organizada en los asuntos que afectan su " +
            "territorio. Su propósito es identificar problemas, proponer soluciones y " +
            "tomar decisiones que beneficien a todas las personas. Escuchar a quienes " +
            "viven el problema es el primer paso para una solución justa.",

            "Participar tiene beneficios concretos: mejora la convivencia, cuida los " +
            "espacios públicos y fortalece el sentido de pertenencia. Un parque limpio " +
            "no es solo un resultado estético: es la prueba de que la comunidad se " +
            "reconoce como responsable de lo que es de todas y todos.",

            "Hay muchas formas de participar: asambleas, jornadas de trabajo, grupos " +
            "vecinales, actividades culturales y deportivas, y la recuperación de " +
            "espacios públicos. El trabajo en equipo y la toma de decisiones colectivas " +
            "permiten soluciones más adecuadas y sostenibles que las impuestas desde afuera.",

            "Cuando una comunidad colabora para recuperar un lugar deteriorado, se " +
            "fortalecen la convivencia, la inclusión, la cooperación y la sostenibilidad. " +
            "Las mejoras duran más porque las personas se sienten dueñas del proceso " +
            "y del resultado.",

            "El Proyecto Urbano Integral (PUI) de Medellín, desarrollado entre 2004 y 2011, " +
            "es un ejemplo educativo real. Buscó articular dimensiones físicas, sociales e " +
            "institucionales, e involucrar a las comunidades en la transformación de sus " +
            "territorios. Este videojuego se inspira en esa idea, pero las misiones del " +
            "barrio son ficticias y no forman parte del proyecto histórico.",

            "Has completado el recorrido. La participación comunitaria no es un acto " +
            "único: es una práctica cotidiana de escucha, colaboración y decisión conjunta."
        };

        public static readonly string ResumenAprendizajes =
            "Aprendizajes\n\n" +
            "• Participar es involucrarse de manera activa en los asuntos del territorio.\n" +
            "• Escuchar a las personas afectadas mejora las decisiones.\n" +
            "• Cuidar lo público fortalece la convivencia y el sentido de pertenencia.\n" +
            "• Las asambleas y el trabajo en equipo producen soluciones más sostenibles.\n" +
            "• El PUI de Medellín (2004-2011) muestra cómo articular lo físico, lo social " +
            "y lo institucional con la comunidad.";
    }
}
