using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ITagRepository
    {
        List<Tag> GetAllTags();

        List<Tag> GetTagzByPostId(int postId);

        void AddTag(Tag tag);

        Tag GetTagById(int id);
        void DeleteTag(int id);

        void UpdateTag(Tag tag);

    }
}
