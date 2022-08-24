using ConsumoApi;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace consumoApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await ConsultarListaEmpregado();
            Console.ReadLine();
            

        }

        public static async Task ConsultarListaEmpregado()
        {
            HttpClient empregado = new HttpClient();

            var response = await empregado.GetAsync($"https://localhost:44345/employee");
            string responseBodyAsText = await response.Content.ReadAsStringAsync();
            var empregados = JsonSerializer.Deserialize<List<Employee>>(responseBodyAsText);

            Console.WriteLine("TABELA DE DADOS - FUNCIONÁRIOS ");
            Console.WriteLine(" ID  -  NOME - GENERO - IDADE - EMAIL -  LOGIN\n");
            foreach (var emp in empregados)
            {
               
                
               Console.WriteLine($"{emp.id}  -  {emp.first_name} {emp.last_name}  -   {emp.gender}    -   {emp.age}    -  {emp.email}  -  {emp.login} ");

            }


           







        }
    }
}


