using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAccess.Services.Interface
{
    public interface IBlogService
    {
        List<Blog> GetAllBlogs();
    }
}
