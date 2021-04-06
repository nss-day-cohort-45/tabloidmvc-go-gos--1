using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class TagRepository : BaseRepository, ITagRepository
    {
        public TagRepository(IConfiguration config) : base(config) { }

        public List<Tag> GetAllTags()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, Name
                        FROM Tag
                        ORDER BY Name

                    ";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Tag> tags = new List<Tag>();
                    while (reader.Read())
                    {
                        Tag tag = new Tag
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };

                        tags.Add(tag);
                    }

                    reader.Close();

                    return tags;
                }
            }
        }
        public void AddTag(Tag tag)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO Tag ([Name]) 
                    OUTPUT INSERTED.ID
                    VALUES (@name);";

                    cmd.Parameters.AddWithValue("@name", tag.Name);

                    int id = (int)cmd.ExecuteScalar();

                    tag.Id = id;


                }
            }
        }
        public Tag GetTagById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, Name
                        FROM Tag
                        WHERE Id = @id
                    ";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Tag tag = new Tag
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };

                        reader.Close();
                        return tag;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }
                }
            }
        }


        public List<Tag> GetTagzByPostId(int postId) {

            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            SELECT t.Name, p.Id
                            FROM Tag t
                            LEFT JOIN PostTag pt on t.Id = pt.TagId
                            LEFT JOIN Post p on pt.PostId = p.Id
                            WHERE p.Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", postId);
                    SqlDataReader reader = cmd.ExecuteReader();



                    List<Tag> tagz = new List<Tag>();
                    while (reader.Read())
                    {
                        Tag tag = new Tag
                        {
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                        tagz.Add(tag);
                    }

                    reader.Close();

                    return tagz;


                }
            }

        }


        public void DeleteTag(int tagId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Tag
                            WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", tagId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdateTag(Tag tag){
            using (SqlConnection conn = Connection)
             {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                      UPDATE Tag
                                      SET
                                            [Name] = @name
                                      WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@name", tag.Name);
                    cmd.Parameters.AddWithValue("@id", tag.Id);

                    cmd.ExecuteNonQuery();

                }
             }
        }    
     }
 }
        
    


