using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ControllersKinect.service
{
    class ApiConec
    {

        public static async Task<string> getCharacterApi()
        {
            string apiUrl = "http://localhost:3000/Servidor/buscar";
            // Crear una instancia de HttpClient
            using (HttpClient client = new HttpClient())
            {
                // Realizar la solicitud GET y obtener la respuesta
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                // Verificar si la solicitud fue exitosa (código de estado 200)
                if (response.IsSuccessStatusCode)
                {
                    // Leer el contenido de la respuesta como una cadena
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Devolver la respuesta
                   return Regex.Replace(responseBody, "(?<!_)\"", string.Empty);
                   // return responseBody.Replace("\"", "");
                }
                else
                {
                    throw new Exception("La solicitud no fue exitosa. Código de estado: " + response.StatusCode);
                }
            }
        }

        public static async Task<string> PostJson( object data)
        {

            string apiUrl = "http://localhost:3000/Servidor/prolog";
            // Serializar el objeto JSON a una cadena
            string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            Console.WriteLine(jsonData);
            // Crear una instancia de HttpClient
            using (HttpClient client = new HttpClient())
            {

               // Crear el contenido de la solicitud con el objeto JSON
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                
                // Agregar el encabezado Content-Type al objeto HttpContent
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                // Realizar la solicitud POST y obtener la respuesta
                HttpResponseMessage response = await client.PostAsync(apiUrl,content);

                // Verificar si la solicitud fue exitosa (código de estado 200)
                if (response.IsSuccessStatusCode)
                {
                    // Leer el contenido de la respuesta como una cadena
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Devolver la respuesta
                    return responseBody;
                }
                else
                {
                    throw new Exception("La solicitud no fue exitosa. Código de estado: " + response.StatusCode);
                }
            }
        }
    }
}