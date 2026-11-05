using DungeonCrawlerMitHandy.Core;
using DungeonCrawlerMitHandy.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DungeonCrawlerMitHandy.Data
{
    public static class DataReader
    {
        /* Konkrete Gegner sind in enemies.json definiert. Die Methode liest sie ein und erstellt
         * blueprints von diesen Gegnern */

        public static Dictionary<EnemyType, EnemyDefinition> LoadEnemyDefinitions(string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            string json = File.ReadAllText(filePath);

            if (string.IsNullOrWhiteSpace(json))
                throw new InvalidDataException($"{fileName}: Die Datei ist leer.");

            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());

            List<EnemyDefinition>? definitions;
            try
            {
                definitions = JsonSerializer.Deserialize<List<EnemyDefinition>>(json, options);
            }
            catch (JsonException ex)
            {
                throw new InvalidDataException($"{fileName}: Kein gültiges JSON. {ex.Message}", ex);
            }

            if (definitions is null)
                throw new InvalidDataException($"{fileName}: Enthält keine Gegnerliste.");

            Dictionary<EnemyType, EnemyDefinition> definitionsByType = new();

            foreach (EnemyDefinition definition in definitions)
            {
                if (!Enum.IsDefined(definition.Type))
                    throw new InvalidDataException($"{fileName}: Unbekannter Gegnertyp \"{definition.Type}\".");

                string prefix = $"{fileName} – {definition.Type}";

                if (definitionsByType.ContainsKey(definition.Type))
                    throw new InvalidDataException($"{prefix}: Dieser Typ ist mehrfach definiert.");
                if (string.IsNullOrWhiteSpace(definition.Name))
                    throw new InvalidDataException($"{prefix}: Name fehlt.");
                if (definition.MinHealth <= 0)
                    throw new InvalidDataException($"{prefix}: MinHealth muss größer als 0 sein (ist {definition.MinHealth}).");
                if (definition.MinHealth > definition.MaxHealth)
                    throw new InvalidDataException($"{prefix}: MinHealth ({definition.MinHealth}) ist größer als MaxHealth ({definition.MaxHealth}).");
                if (definition.AttackDamage < 0)
                    throw new InvalidDataException($"{prefix}: AttackDamage darf nicht negativ sein (ist {definition.AttackDamage}).");

                definitionsByType.Add(definition.Type, definition);
            }

            foreach (EnemyType type in Enum.GetValues<EnemyType>())
            {
                if (!definitionsByType.ContainsKey(type))
                    throw new InvalidDataException($"{fileName}: Für den Gegnertyp \"{type}\" fehlt ein Eintrag.");
            }

            return definitionsByType;
        }
    }
}
