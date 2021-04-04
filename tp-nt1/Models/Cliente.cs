﻿using System;
using System.Collections.Generic;

namespace tp_nt1.Models
{
    public class Cliente : Persona
    {
        #region Propiedades
        public string Dni { get; private set; }
        public List<Compra> Compras { get; private set; }
        public List<Carrito> Carritos { get; private set; }
        #endregion
        #region Constructores
        public Cliente(string dni)
        {
            //¿Como reflejar el constructor de Persona?
            Dni = dni;
            Compras = new List<Compra>();
            Carritos = new List<Carrito>();
        }
        public Cliente() : this("Sin Asignar")
        {
        }
        #endregion
    }
}