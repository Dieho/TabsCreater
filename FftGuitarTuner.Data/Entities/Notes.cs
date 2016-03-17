namespace FftGuitarTuner.Data.Entities
{
    public class Notes: IEntity
    {
        public int Id { get; set; }
        public string Note { get; set; }
        public double Frequency { get; set; }
    }
}
