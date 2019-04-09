using AutoMapper;
using BusinessAccess.Repository;
using BusinessAccess.Services.Interface;
using DataAccess.Model;
using log4net;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessAccess.Services.Implement
{
    public class BlogService : IBlogService
    {
        private readonly IRepository<Blog> _blogRepo;
        private readonly IMapper _mapper;
        private readonly ILog _logger;
        private readonly IConfiguration _configuration;

        public BlogService(IRepository<Blog> blogRepo, IMapper mapper, IConfiguration configuration)
        {
            _blogRepo = blogRepo;
            _logger = LogManager.GetLogger(typeof(BlogService));
            _configuration = configuration;
        }

        public List<Blog> GetAllBlogs()
        {
            var myName = _configuration["MyName"];
            _logger.Info(new { Code = "1", Message = $"Configuration key MyName: {myName}" });

            var result = _blogRepo.GetAll().ToList();
            if (result.Count <= 0)
                return new List<Blog>();
            // Can Keep Demo AutoMapper
            return _mapper.Map<List<Blog>, List<Blog>>(result);
        }
    }
}
