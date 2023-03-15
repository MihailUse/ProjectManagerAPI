﻿namespace Application.DTO.User;

public class UserBriefDto
{
    public Guid Id { get; set; }
    public Guid AvatarId { get; set; }
    public string Login { get; set; } = null!;
}