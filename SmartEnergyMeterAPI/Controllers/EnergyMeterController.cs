using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartEnergyMeterAPI.Data;
using SmartEnergyMeterAPI.Models;

namespace SmartEnergyMeterAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnergyMeterController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EnergyMeterController> _logger;

        public EnergyMeterController(ApplicationDbContext context, ILogger<EnergyMeterController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // POST: api/EnergyMeter/reading
        [HttpPost("reading")]
        public async Task<ActionResult<EnergyReading>> PostEnergyReading(EnergyReadingDto readingDto)
        {
            try
            {
                var reading = new EnergyReading
                {
                    DateTime = DateTime.UtcNow,
                    Voltage = readingDto.Voltage,
                    Current = readingDto.Current,
                    Power = readingDto.Power,
                    Frequency = readingDto.Frequency,
                    PowerFactor = readingDto.PowerFactor,
                    DeviceId = readingDto.DeviceId ?? "ESP8266_01"
                };

                _context.EnergyReadings.Add(reading);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Energy reading saved: {reading.Id}");
                return CreatedAtAction(nameof(GetEnergyReading), new { id = reading.Id }, reading);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving energy reading");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/EnergyMeter/reading/{id}
        [HttpGet("reading/{id}")]
        public async Task<ActionResult<EnergyReading>> GetEnergyReading(int id)
        {
            var reading = await _context.EnergyReadings.FindAsync(id);
            if (reading == null)
            {
                return NotFound();
            }
            return reading;
        }

        // GET: api/EnergyMeter/readings
        [HttpGet("readings")]
        public async Task<ActionResult<IEnumerable<EnergyReading>>> GetEnergyReadings(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50,
            [FromQuery] string? deviceId = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var query = _context.EnergyReadings.AsQueryable();

            if (!string.IsNullOrEmpty(deviceId))
                query = query.Where(r => r.DeviceId == deviceId);

            if (fromDate.HasValue)
                query = query.Where(r => r.DateTime >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(r => r.DateTime <= toDate.Value);

            var readings = await query
                .OrderByDescending(r => r.DateTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return readings;
        }

        // GET: api/EnergyMeter/readings/latest
        [HttpGet("readings/latest")]
        public async Task<ActionResult<EnergyReading>> GetLatestReading(string? deviceId = null)
        {
            var query = _context.EnergyReadings.AsQueryable();

            if (!string.IsNullOrEmpty(deviceId))
                query = query.Where(r => r.DeviceId == deviceId);

            var latest = await query
                .OrderByDescending(r => r.DateTime)
                .FirstOrDefaultAsync();

            if (latest == null)
                return NotFound();

            return latest;
        }

        // GET: api/EnergyMeter/readings/csv
        [HttpGet("readings/csv")]
        public async Task<IActionResult> GetReadingsAsCsv(
            [FromQuery] string? deviceId = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var query = _context.EnergyReadings.AsQueryable();

            if (!string.IsNullOrEmpty(deviceId))
                query = query.Where(r => r.DeviceId == deviceId);

            if (fromDate.HasValue)
                query = query.Where(r => r.DateTime >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(r => r.DateTime <= toDate.Value);

            var readings = await query
                .OrderBy(r => r.DateTime)
                .ToListAsync();

            var csv = "DateTime,Voltage,Current,Power,Frequency,PowerFactor,DeviceId\n";
            csv += string.Join("\n", readings.Select(r =>
                $"{r.DateTime:yyyy-MM-dd HH:mm:ss},{r.Voltage},{r.Current},{r.Power},{r.Frequency},{r.PowerFactor},{r.DeviceId}"));

            return File(System.Text.Encoding.UTF8.GetBytes(csv), "text/csv", $"energy_readings_{DateTime.Now:yyyyMMdd}.csv");
        }
    }
}
