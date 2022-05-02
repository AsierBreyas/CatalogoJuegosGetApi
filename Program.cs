using System;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using NW = Newtonsoft.Json;
using MS = System.Text.Json;
using System.Collections.Generic;
using Juegos.Models;

class Program
{
    static async Task Main(string[] args)
    {
        TimeSpan interval = new TimeSpan(0, 0, 600);
        try
        {
            Console.WriteLine("empieza");
        }
        catch (Exception a)
        {
            Console.WriteLine("error=>" + a);
        }
        while (true)
        {
            await TareaAsincrona();
            Thread.Sleep(interval);
        }
    }

    static async Task TareaAsincrona()
    {
        var client = new HttpClient();
        var UrlListadoJuegos = $"https://www.freetogame.com/api/games";
        HttpResponseMessage RespuestaHTTPApiJuegos = await client.GetAsync(UrlListadoJuegos);
        var ContenidoRespApiJuegos = await RespuestaHTTPApiJuegos.Content.ReadAsStringAsync();
        dynamic objetoJsonDeserializadoJuegos = NW.JsonConvert.DeserializeObject(
            ContenidoRespApiJuegos
        );
        var vObjetoJsonDeserializadoJuegos = objetoJsonDeserializadoJuegos;
        foreach (var juego in vObjetoJsonDeserializadoJuegos)
        {
            dynamic pId = juego.id;
            dynamic pTitulo = juego.title;
            dynamic pImg = juego.thumbnail;
            dynamic pPlataforma = juego.platform;
            dynamic pSalida = juego.release_date;
            Console.WriteLine($" ");
            Console.WriteLine($"****************************************************************");
            Console.WriteLine($"{pId}--{pTitulo}--{pImg}--{pPlataforma}--{pSalida}");
            Console.WriteLine($"****************************************************************");
            Console.WriteLine("====");

            var UrlListadoInfoJuegos = $"https://www.freetogame.com/api/game?id={juego.id}";
            HttpResponseMessage RespuestaHTTPApiInfoJuegos = await client.GetAsync(
                UrlListadoInfoJuegos
            );
            var ContenidoRespApiInfoJuegos =
                await RespuestaHTTPApiInfoJuegos.Content.ReadAsStringAsync();
            dynamic objetoJsonDeserializadoInfoJuegos = NW.JsonConvert.DeserializeObject(
                ContenidoRespApiInfoJuegos
            );
            var vObjetoJsonDeserializadoInfoJuegos = objetoJsonDeserializadoInfoJuegos;
            foreach (var juegoInfo in vObjetoJsonDeserializadoInfoJuegos)
            {
                dynamic sAScreenshots = "";
                foreach (var scrsot in vObjetoJsonDeserializadoInfoJuegos.screenshots)
                {
                    sAScreenshots += Convert.ToString(scrsot.image) + ",";
                }
                if(sAScreenshots.Length!=0)sAScreenshots = sAScreenshots.Substring(0, sAScreenshots.Length-1);
                using (var db = new JuegosCatalogoContext())
                {
                    try
                    {
                        string titulo = pTitulo + "";
                        int identif = pId;
                        var row = db.CatalogoJuegos.Where(a => a.JuegoId == identif).SingleOrDefault();
                        if (row == null)
                        {
                            var a1 = new CatalogoJuegos
                            {
                                JuegoId = pId,
                                title = titulo,
                                thumbnail = pImg,
                                short_description = juego.short_description,
                                description = vObjetoJsonDeserializadoInfoJuegos.description,
                                game_url = juego.game_url,
                                genre = juego.genre,
                                plataform = juego.platform,
                                publisher = juego.publisher,
                                developer = juego.developer,
                                release_date = juego.release_date,
                                freetogame_profile_url = juego.freetogame_profile_url,
                                screenshots = sAScreenshots,
                                os = vObjetoJsonDeserializadoInfoJuegos.minimum_system_requirements.os,
                                processor = vObjetoJsonDeserializadoInfoJuegos.minimum_system_requirements.processor,
                                memory = vObjetoJsonDeserializadoInfoJuegos.minimum_system_requirements.memory,
                                graphics = vObjetoJsonDeserializadoInfoJuegos.minimum_system_requirements.graphics,
                                storage = vObjetoJsonDeserializadoInfoJuegos.minimum_system_requirements.graphics
                            };
                            db.CatalogoJuegos.Add(a1);
                        }
                        else
                        {
                            row.JuegoId = pId;
                            row.title = titulo;
                            row.thumbnail = pImg;
                            row.short_description = juego.short_description;
                            row.description = vObjetoJsonDeserializadoInfoJuegos.description;
                            row.game_url = juego.game_url;
                            row.genre = juego.genre;
                            row.plataform = juego.platform;
                            row.publisher = juego.publisher;
                            row.developer = juego.developer;
                            row.release_date = juego.release_date;
                            row.freetogame_profile_url = juego.freetogame_profile_url;
                            row.screenshots = sAScreenshots;
                            row.os = vObjetoJsonDeserializadoInfoJuegos.minimum_system_requirements.os;
                            row.processor = vObjetoJsonDeserializadoInfoJuegos.minimum_system_requirements.processor;
                            row.memory = vObjetoJsonDeserializadoInfoJuegos.minimum_system_requirements.memory;
                            row.graphics = vObjetoJsonDeserializadoInfoJuegos.minimum_system_requirements.graphics;
                            row.storage = vObjetoJsonDeserializadoInfoJuegos.minimum_system_requirements.storage;
                        }
                        db.SaveChanges();
                    }
                    catch (Exception p)
                    {
                        Console.WriteLine("No se ha podido guardar por " + p);
                    }
                }
            }
        }
    }
}
