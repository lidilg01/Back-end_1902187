namespace WebApiBooks.Services
{
    public class TimerPunto3 : IHostedService
    {
        private readonly IWebHostEnvironment env;
        private readonly string nombreArchivo = "MensajeParaElProfe.txt";
        private Timer timer;

        public TimerPunto3(IWebHostEnvironment env)
        {
            this.env = env;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //Se ejecuta cuando cargamos la aplicacion 1 vez
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(120));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Se ejecuta cuando detenemos la aplicacion aunque puede que no se ejecute por algun error. 
            timer.Dispose();
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            Escribir("El Profe Gustavo Rodriguez es el mejor");
        }
        private void Escribir(string msg)
        {
            var ruta = $@"{env.ContentRootPath}\wwwroot\{nombreArchivo}";
            using (StreamWriter writer = new StreamWriter(ruta, append: true)) { writer.WriteLine(msg); }
        }
    }
}
