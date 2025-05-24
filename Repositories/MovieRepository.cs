using app_movie_server.Data;
using app_movie_server.Models;
using app_movie_server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace app_movie_server.Repositories
{
    /// <summary>
    /// Repositorio para la gestión de entidades <see cref="Movie"/> en la base de datos.
    /// </summary>
    /// <remarks>
    /// Esta clase implementa la interfaz <see cref="IMovieRepository"/> y utiliza <see cref="AppDbContext"/> para interactuar con la base de datos.
    /// </remarks>
    public class MovieRepository(AppDbContext context) : IMovieRepository
    {
        private readonly AppDbContext _context = context;

        public bool CreateMovie(Movie movie)
        {
            movie.CreatedAt = DateTime.Now;

            _context.Movies.Add(movie);

            return Save();
        }

        public bool DeleteMovie(Movie movie)
        {
            _context.Movies.Remove(movie);

            return Save();
        }

        public Movie GetMovie(int movieId)
        {
            return _context.Movies.FirstOrDefault(m => m.Id == movieId)!;
        }

        public ICollection<Movie> GetMovies(int pageNumber, int pageSize)
        {
            return [.. _context.Movies
                .OrderBy(m => m.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)];
        }

        public ICollection<Movie> GetMoviesByCategory(int categoryId)
        {
            return [.. _context.Movies.Include(m => m.Category).Where(m => m.CategoryId == categoryId)];
        }

        public int GetTotalMovies()
        {
            return _context.Movies.Count();
        }

        public bool MovieExists(int movieId)
        {
            return _context.Movies.Any(m => m.Id == movieId);
        }

        public bool MovieExists(string name)
        {
            return _context.Movies.Any(m => m.Name == name);
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }

        public IEnumerable<Movie> SearchMovie(string name)
        {
            IQueryable<Movie> query = _context.Movies;

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(m => m.Name!.ToLower().Contains(name.ToLower()));
            }

            return [.. query];
        }

        public bool UpdateMovie(Movie movie)
        {
            movie.CreatedAt = DateTime.Now;

            var existingMovie = _context.Movies.Find(movie.Id);

            if (existingMovie != null)
            {
                _context.Entry(existingMovie).CurrentValues.SetValues(movie);
            }
            else
            {
                _context.Movies.Update(movie);
            }

            return Save();
        }
    }
}