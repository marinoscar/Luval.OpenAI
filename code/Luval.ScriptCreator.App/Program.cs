namespace Luval.ScriptCreator.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var conn = "Server=.\\SQLEXPRESS;Database=AdventureWorks2019;Trusted_Connection=True;";
            var script = new Script(conn);
            var content = script.CreateFullScript();
            var resumed = script.GetCompressedScript();

            File.WriteAllText("complete.sql", content);
            File.WriteAllText("small.sql", resumed);

        }
    }
}