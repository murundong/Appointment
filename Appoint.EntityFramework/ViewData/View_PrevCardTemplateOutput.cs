using Appoint.EntityFramework.Data;
using BaseClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.EntityFramework.ViewData
{
    [NotMapped]
    public class View_PrevCardTemplateOutput: CardTemplate
    {
        private string _door_img;
        public string door_img
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_door_img))
                {
                    return ConfigurationHelper.GetAppSetting<string>("ErrorImg");
                }
                return _door_img;
            }
            set
            {
                _door_img = value;
            }
        }
    }
}
