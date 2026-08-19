using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IndexadorIA.Entidades;
using Microsoft.IdentityModel.Tokens;

namespace IndexadorIA.Api.Servicios
{
    /// <summary>
    /// Genera los tokens JWT que la API entrega tras un login exitoso.
    /// </summary>
    public class JwtServicio
    {
        private readonly IConfiguration _configuracion;

        public JwtServicio(IConfiguration configuracion)
        {
            _configuracion = configuracion;
        }

        public string GenerarToken(Usuario usuario)
        {
            string claveSecreta = _configuracion["SeguridadApi:Jwt:ClaveSecreta"]
                ?? throw new InvalidOperationException("Falta configurar SeguridadApi:Jwt:ClaveSecreta");
            string emisor = _configuracion["SeguridadApi:Jwt:Emisor"] ?? "IndexadorIA.Api";
            string audiencia = _configuracion["SeguridadApi:Jwt:Audiencia"] ?? "IndexadorIA.Cliente";
            double horasExpiracion = double.TryParse(_configuracion["SeguridadApi:Jwt:ExpiracionHoras"], out var h) ? h : 8;

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, usuario.CdUsuario.ToString()),
                new(ClaimTypes.NameIdentifier, usuario.CdUsuario.ToString()),
                new(ClaimTypes.Name, usuario.DsUsuario),
                new(ClaimTypes.Role, usuario.CdRol),
                new("dsNombreCompleto", usuario.DsNombreCompleto)
            };

            var credenciales = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(claveSecreta)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: emisor,
                audience: audiencia,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(horasExpiracion),
                signingCredentials: credenciales);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
