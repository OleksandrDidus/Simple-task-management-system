﻿namespace WEBTask.Models.DTO
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public IEnumerable<TaskDto> Tasks { get; set; } = new List<TaskDto>();
    }
}
