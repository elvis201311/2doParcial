using _2doParcial.DAL;
using _2doParcial.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2doParcial.BLL
{
    public class TareasBLL
    {
        
        public static List<Tareas> GetList()
        {
            List<Tareas> tareas = new List<Tareas>();
            Contexto contexto = new Contexto();

            try
            {
                tareas = contexto.Tareas.ToList();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }

            return tareas;
        }
    }
}