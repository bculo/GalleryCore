using ApplicationCore.Entities;
using System;

namespace ApplicationCore.Services.Helpers
{
    public class UserValidation
    {
        /// <summary>
        /// Check if username or password is null or empty
        /// </summary>
        /// <param name="user"></param>
        public virtual void CheckParametarsOnLogin(Uploader user)
        {
            if (string.IsNullOrWhiteSpace(user.UserName))
            {
                throw new ArgumentNullException(nameof(user.UserName));
            }

            /*
            if (string.IsNullOrWhiteSpace(user.Password))
            {
                throw new ArgumentNullException(nameof(user.Password));
            }
            */
        }

        public virtual void CheckUserOnEmailActivation(Uploader user, string url)
        {
            /*
            if (string.IsNullOrWhiteSpace(user?.Email ?? null))
            {
                throw new ArgumentNullException(nameof(user.Email));
            }
            */
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }
        }

        /// <summary>
        /// Check if username, password or email is empty or null
        /// </summary>
        /// <param name="user"></param>
        public virtual void CheckUserOnStartOfRegistration(Uploader user)
        {
            if (string.IsNullOrWhiteSpace(user.UserName))
            {
                throw new ArgumentNullException(nameof(user.UserName));
            }

            /*
            if (string.IsNullOrWhiteSpace(user.Password))
            {
                throw new ArgumentNullException(nameof(user.Password));
            }

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new ArgumentNullException(nameof(user.Email));
            }
            */
        }
    }
}
