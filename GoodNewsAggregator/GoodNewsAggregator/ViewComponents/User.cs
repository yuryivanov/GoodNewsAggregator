using GoodNewsAggregator.DAL.Repositories.Implementation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsAggregator.ViewComponents
{
    public class User : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public User(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> InvokeAsync()
        {
            if (HttpContext.User.Identity.Name != null)
            {
                var userEmail = HttpContext.User.Identity.Name;
                var user = _unitOfWork.Users.FindBy(user => user.Email.Equals(userEmail)).FirstOrDefault();
                var userRoleName = (_unitOfWork.Roles.FindBy(role => role.Id.Equals(user.RoleId)).FirstOrDefault()).Name;


                if (userRoleName == "Admin")
                {
                    string userName = HttpContext.User.Identity.Name;
                    return $"{user.FullName} - {userName}";
                }
                else if (userRoleName == "User")
                {
                    string userName = HttpContext.User.Identity.Name;
                    return $"{user.FullName} - {userName}";
                }
                else
                {
                    return "Гость";
                }
            }
            else
            {
                return "Гость";
            }
        }
    }
}
