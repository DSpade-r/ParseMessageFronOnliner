using System.ComponentModel.DataAnnotations;
namespace UserMessages.Models
{
    public class ParseInfo
    {
        [Required(ErrorMessage = "Введите никнейм пользователя")]
        public string Name { set; get; }
        public int UserID { set; get; }
        [Required(ErrorMessage = "Введите макимальное количество сообщений для поимка")]
        [Range(1,5000,ErrorMessage ="Количество сообщений 1-5000")]
        public int MaxMessage { set; get; }
        [Required(ErrorMessage = "Введите количество просматриваемых страниц")]
        [Range(1,1000,ErrorMessage = "Количество страниц 1-1000")]
        public int PageCount { set; get; }
        [Required(ErrorMessage = "Введите адрес начальной страницы просмотра сообщений")]
        public string url { set; get; }
    }
}