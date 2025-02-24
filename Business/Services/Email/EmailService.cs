using Azure.Core;
using Business.Interfaces;
using Infrastructure.Contexts;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Email
{
    public class EmailService : IServiceEmail
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _applicationDbContext;
        public EmailService(IConfiguration configuration, ApplicationDbContext applicationDbContext)
        {
            this._configuration = configuration;
            _applicationDbContext = applicationDbContext;
        }

        private string BODY_BIENVENIDA = @"
<p>Estimado/a,</p>
<p>Le damos la bienvenida a <strong>GymShift</strong>!! 💪🏽</p>
<p>El motivo del correo es para comunicarle que su Usuario es: <strong>[USUARIO]</strong> y su clave es: <strong>[CLAVE]</strong></p>
<p>Le sugerimos modificar la clave por la que usted desee.</p>
<p>Atentamente,<br><strong>El equipo de GymShift 😊</strong></p>";
        private const string MAIL_WELCOME = "EMAIL_BIENVENIDA";
        public async Task EnvioMail(string emailReceptor, string constAsunto, string clave, string usuario, string nombre)
        {
            var emailEmisor = _configuration.GetValue<string>("CONFIGURACIONES_EMAIL:EMAIL");
            var password = _configuration.GetValue<string>("CONFIGURACIONES_EMAIL:PASSWORD");
            var host = _configuration.GetValue<string>("CONFIGURACIONES_EMAIL:HOST");
            var puerto = _configuration.GetValue<int>("CONFIGURACIONES_EMAIL:PUERTO");

            if (constAsunto == MAIL_WELCOME) constAsunto = $"Bienvenido/a {nombre} a GymShift!!";
            var smtpCliente = new SmtpClient(host, puerto);
            smtpCliente.EnableSsl = true;
            smtpCliente.UseDefaultCredentials = false;
            smtpCliente.Credentials = new NetworkCredential(emailEmisor, password);

            string bodyConClave = BODY_BIENVENIDA.Replace("[USUARIO]", usuario)
            .Replace("[CLAVE]", clave);

            var mensaje = new MailMessage
            {
                From = new MailAddress(emailEmisor!),
                Subject = constAsunto,
                Body = bodyConClave,
                IsBodyHtml = true
            };

            mensaje.To.Add(emailReceptor);

            await smtpCliente.SendMailAsync(mensaje);

            await InsertMailToDB(emailReceptor, constAsunto, bodyConClave);

        }

        public async Task InsertMailToDB(string email, string asunto, string body)
        {

            ServicioEmail servicioEmail = new ServicioEmail()
            {
                DescripcionEmail = "Generacion de clave",
                EmailEmisor = "agusantillan16@gmail.com",
                EmailReceptor = email,
                Asunto = asunto,
                Cuerpo = body,
                FechaEnvio = DateTime.Now
            };

            await _applicationDbContext.ServicioEmails.AddAsync(servicioEmail);
            await _applicationDbContext.SaveChangesAsync();
        }

    }
}
