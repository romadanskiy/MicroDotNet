using System;
using AuthorizationServer.Web.Dto;

namespace AuthorizationServer.Web.Controllers;

public static class Validator
{
    public static void UserRegisterDtoValidator(UserRigisterDto user)
    {
        if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password) ||
            string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName) ||
            string.IsNullOrEmpty(user.PhoneNumber))
        {
            throw new ArgumentException("Данные пользователя не содержат всей информации");
        }
    }
}