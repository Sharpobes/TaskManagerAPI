using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Models
{
    public class ProjectViewModel
    {
        /// <summary>
        /// Идентификатор проекта (если нужно отображать или редактировать уже существующий).
        /// Для создания нового проекта может быть не нужен, но часто оставляют для универсальности формы.
        /// </summary>
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле 'Name' обязательно для заполнения.")]
        [StringLength(200, ErrorMessage = "Длина поля 'Name' не должна превышать 200 символов.")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Описание проекта (необязательное поле).
        /// </summary>
        [StringLength(1000, ErrorMessage = "Длина поля 'Description' не должна превышать 1000 символов.")]
        public string? Description { get; set; }

        /// <summary>
        /// Владелец (Id пользователя). Если требуется выбрать из списка пользователей, 
        /// можно отобразить OwnerId и Dropdown в представлении.
        /// </summary>
        [Required(ErrorMessage = "Необходимо указать владельца.")]
        public int OwnerId { get; set; }

        /// <summary>
        /// Дата создания. Если вы хотите, чтобы она формировалась автоматически на сервере (в БД),
        /// поле в форме может быть необязательным или скрытым.
        /// </summary>
        public DateTime? CreatedAt { get; set; }
    }
}
