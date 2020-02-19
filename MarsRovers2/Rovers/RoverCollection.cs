using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using MarsRovers2.Enums;

namespace MarsRovers2.Rovers {
    public sealed class RoverCollection 
        : List<Rover> {

        #region Properties

        public Guid PlateauID { get; private set; }

        // these were hand picked (by myself) from a public NASA poll to name the Curiosity rover
        private List<string> RoverNames { get; } = new List<string>() { "Curiosity", "Pursuit", "Vision" };

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RoverCollection"/> class.
        /// </summary>
        /// <param name="plateauID">The plateau identifier.</param>
        public RoverCollection(Guid plateauID) {
            this.Initialize(plateauID);
        }

        #endregion

        #region Indexers

        /// <summary>
        /// Gets the <see cref="MarsRovers.Rovers.Rover" /> with the specified rover identifier.
        /// </summary>
        /// <param name="roverID">The rover identifier.</param>
        /// <returns>MarsRovers.Rovers.Rover.</returns>
        public Rover this[Guid roverID] {
            get {
                return this.FirstOrDefault(r => r.RoverID == roverID);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a new rover to the collection and returns the newly generated ID.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="direction">The direction.</param>
        /// <returns>Guid.</returns>
        public Guid Add(int x, int y, Directions direction) {
            Guid roverID = Guid.NewGuid(); // this would typically be a database-generated ID
            Random randomGenerator = new Random();
            string roverName = this.RoverNames[randomGenerator.Next(0, this.RoverNames.Count)];
            var rover = new Rover(x, y, roverName, direction);
            this.RoverNames.Remove(roverName);
            this.Add(rover);
            return rover.RoverID;
        }

        private void Initialize(Guid plateauID) {
            // I'm not actually using this property in my code, but this is how I would generally create my collections using ADO.NET 
            //	- pass in the parent ID, call a stored procedure from the Initialize method
            this.PlateauID = plateauID;
            using var connection = new SqlConnection();
            using var command = connection.CreateCommand();
            command.CommandText = "dbo.InitializeRovers";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add($@"@{nameof(PlateauID)}", SqlDbType.UniqueIdentifier).Value = plateauID;

            //Execute reader, etc.
        }

        #endregion

    }
}
