using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace Votaciones_App.Negocio
{
    public class Option
    {

        public string id { get; set; }
        public int totalVotes { get; set; }
        public static Chart chart;      // Estático para que todos los objetos Option compartan un mismo objeto Chart
        public Option(string id)
        {
            this.id = id;
        }

        public string getId()
        {
            return id;
        }

        public void addVote()
        {
            totalVotes++;
            chart.DataBind();           // Relación de los datos con la gráfica al llamar a este método

            checkWinner();
        }

        public void removeVote()
        {
            totalVotes--;
            chart.DataBind();           // Relación de los datos con la gráfica al llamar a este método

            checkWinner();
        }

        private void checkWinner()
        {
            if (this.totalVotes >= VoteManager.maximoParaGanar)
            {
                foreach (DataPoint dataPoint in chart.Series["Votos"].Points)
                {
                    if (dataPoint.AxisLabel == id)
                        dataPoint.Color = Color.LightGreen;
                    else
                        dataPoint.Color = Color.Tomato;
                }
                System.Console.WriteLine("Opcion: " + id + " ha ganado");
            }
        }
    }
}
