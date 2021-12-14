namespace FTPSERVER.Helpers
{
    #region Información funcionalidad de la clase.
    /*
     * Descripcion: Esta clase lleva un historico de logs que se pueden dar en la aplicación, actualmente solo pueden ocurrir cuando sube o descarga un archivo
     * y el caso se de otro tipo de excepción.
     * Procedimiento de la Clase: la clase tiene dos propiedades Path: la ruta, y un salto de linea LienJump para que escriba debajo, cuando el log termine.
     * La propiedad Path es inicializada en el constructor por el parametro del mismo nombre Path, luego tiene un metodo de tipo string GetFileName que obtiene el nombre del 
     * archivo, este metodo tiene una variable FileName, se inicializa con el año, mes y día, y luego se retorna dicha variable.
     * Tiene otro metodo AddLogs que se encarga de registrar los logs en la ruta o Path especificada, llama el metodo CreateDirectory, la variable Name es inicializada
     * con el metodo GetFileName y tiene otra variable StringFormat que se inicializa con la hora actual más el nombre del log mas el ambiente donde corre la app y escribe una 
     * nueva linea.
     * Finalmente tiene el metodo crear directorio el cual como su nombre lo indica verifica si el directorio existe en la ruta especificada,si no existe procede a crearlo en la 
     * ruta. se maneja una exception DirectoryNotFoundException.
     */
    #endregion
    public class LogsHistory
    {
        public  string Path { get; set; } = "";
        public  string LineJump { get; set; } = @"\n";

        public  LogsHistory(string Path)
        {
            this.Path = Path;
        }

        private string GetFileName()
        { 
           var FileName=string.Empty;
            FileName = "Log-" + DateTime.Now.Year.ToString() +"-"+ DateTime.Now.Month.ToString() +"-"+ DateTime.Now.Day.ToString() + ".txt";
            return FileName;
        }

        public void AddLogs(string Log)
        {
            CreateDirectory();
            var Name = GetFileName();
            var StringForma = string.Empty;

            StringForma = DateTime.Now + "-"+ Log + Environment.NewLine;

            var StreamWriter=new StreamWriter(Path+"/"+Name,true);
            StreamWriter.Write(StringForma);
            StreamWriter.Close();
        }
        private void CreateDirectory()
        {
            try
            {
                if (!Directory.Exists(Path))
                    Directory.CreateDirectory(Path);
            }
            catch (DirectoryNotFoundException ex)
            {
                throw new DirectoryNotFoundException(ex.Message);
            }
         
        }
    }
}
