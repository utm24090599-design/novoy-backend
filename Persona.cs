namespace backend
{
    public class Persona
    {
        public int Edad { get; set; }
        public bool EsEstudiante { get; set; }

    }
    public class Costo
    {
        public static decimal CalcularCosto(Persona persona)
        {
            decimal cobro = 0;
            if (persona.Edad < 16)
            {
                cobro = 20;
            }
            else if (persona.Edad > 60)
            {
                cobro = 20;
            }
            else if (persona.EsEstudiante)
            {
                cobro = 50;
            }
            else
            {
                cobro = 100;
            }
            return cobro;
        }
    }
}
