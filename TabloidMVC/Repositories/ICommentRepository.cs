using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Repositories
{
    public interface ICommentRepository
    {
        //void add(Comment comment);
        //void delete(int id);
        //void UpdateComment(Comment comment);
        List<Comment> GetCommentsByPost(int postId);
        //Comment getCommentById(int id);
    }
}
