using BlazorBlogOfPiGettingStarted.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBlogOfPiGettingStarted.Services
{
  public interface IToDoListService
  {
    Task<List<ToDo>> GetAsync();
    Task<ToDo> GetAsync(int id);
    Task<ToDo> AddAsync(ToDo toDo);
    Task<ToDo> UpdateAsync(ToDo toDo);
    Task<ToDo> DeleteAsync(int id);
  }
  public class ToDoListService : IToDoListService
  {
    private readonly ApplicationDbContext _context;
    public ToDoListService(ApplicationDbContext context) => _context = context;
    public async Task<ToDo> AddAsync(ToDo toDo)
    {
      _context.ToDoList.Add(toDo);
      await _context.SaveChangesAsync();
      return toDo;

    }
    public async Task<ToDo> DeleteAsync(int id)
    {
      var toDo = await _context.ToDoList.FindAsync(id);
      _context.ToDoList.Remove(toDo);
      await _context.SaveChangesAsync();
      return toDo;
    }
    public async Task<List<ToDo>> GetAsync() => await _context.ToDoList.ToListAsync();
    public async Task<ToDo> GetAsync(int id) => await _context.ToDoList.FirstAsync(t => t.Id == id);
    public async Task<ToDo> UpdateAsync(ToDo toDo)
    {
      _context.Entry(toDo).State = EntityState.Modified;
      await _context.SaveChangesAsync();
      return toDo;
    }

  }
}
