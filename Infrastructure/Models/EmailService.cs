using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class EmailService
    {
        public int Id { get; set; }
        public string EmailDescription { get; set; }
        public string emailSender { get; set; }
        public string EmailReceptor { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime DateSent { get; set; }
    }
}
