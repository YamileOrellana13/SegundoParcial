using System;
using System.Collections.Generic;
using System.Text;

namespace App.Models
{
    public class Response               // si ejecuta va decirme si se ejecuto sio no
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }
    }
}