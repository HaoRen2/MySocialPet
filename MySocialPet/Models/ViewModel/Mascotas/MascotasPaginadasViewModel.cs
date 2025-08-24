namespace MySocialPet.Models.ViewModel.Mascotas
{
    public class MascotasPaginadasViewModel
    {
        public List<ListaMascotaViewModel> Mascotas { get; set; }

        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }


        public bool TienePaginaAnterior => PaginaActual > 1;
        public bool TienePaginaSiguiente => PaginaActual < TotalPaginas;
    }
}
