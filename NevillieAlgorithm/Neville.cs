using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NevillieAlgorithm
{
    public class Neville
    {
        public MassPoint[] m_ControlPoints;
        public List<int> m_Indices;
        public Neville()
        {

        }
        public Neville(MassPoint[] massPoints)
        {
            m_ControlPoints = massPoints;
            m_Indices = new List<int>();

            for (var i = 0; i < m_ControlPoints.Length; ++i)
            {
                m_Indices.Add(i);
            }
        }

        public Neville(string input_file)
        {
            init(input_file);
        }

        public void init(string path)
        {
            Console.WriteLine(path);
            string[] lines = System.IO.File.ReadAllLines(path);

            // No input; return
            if (lines.Length == 0) return;

            // Create the control points & indices 
            var count = Int32.Parse(lines[0]);
            m_ControlPoints = new MassPoint[count];
            m_Indices = new List<int>();

            // Remove the first line (amount of control points)
            lines = lines.Skip(1).ToArray();

            // Set variables
            for (int i = 0; i < lines.Length; ++i)
            {
                // Split by space; Get all the inputs
                var variables = lines[i].Split(' ');
                m_ControlPoints[i] = new MassPoint();

                // Set variables
                set_mass(ref m_ControlPoints[i], variables);
                if (set_position(ref m_ControlPoints[i], variables))
                    m_Indices.Add(i);
                if (set_velocity(ref m_ControlPoints[i], variables))
                    m_Indices.Add(i);
                if (set_acceleration(ref m_ControlPoints[i], variables))
                    m_Indices.Add(i);
            }
        }

        // Only odd length string have mass info
        // Last variable is the mass info
        private void set_mass(ref MassPoint mp, string[] input)
        {
            if (input.Length % 2 == 0) return;

            mp.mass = Int32.Parse(input[input.Length - 1]);
        }

        // 0 1 will be position x y
        private bool set_position(ref MassPoint mp, string[] input)
        {
            if (input.Length < 2) return false;
            
            mp.position = new Vector2(Int32.Parse(input[0]), Int32.Parse(input[1]));

            // There is mass
            if (input.Length % 2 != 0) mp.position *= mp.mass;

            return true;
        }

        // 2 3 will be velocity x y
        private bool set_velocity(ref MassPoint mp, string[] input)
        {
            if (input.Length < 4) return false;

            mp.velocity = new Vector2(Int32.Parse(input[2]), Int32.Parse(input[3]));
            // There is mass
            if (input.Length % 2 != 0) mp.velocity *= mp.mass;

            return true;
        }

        // 4 5 will be acceleration x y
        private bool set_acceleration(ref MassPoint mp, string[] input)
        {
            if (input.Length < 6) return false;
           
            mp.acceleration = new Vector2(Int32.Parse(input[4]), Int32.Parse(input[5]));
            // There is mass
            if (input.Length % 2 != 0) mp.acceleration *= mp.mass;

            return true;
        }

        private MassPoint TestVelocity(ref float rhs_coefficient, float t, float t0, float t1, float deno, int index, List<int> indices)
        {
            // Not velocity
            if (indices[0] != indices[1])
            {
                return m_ControlPoints[index] * ((t1 - t) / deno);
            }
            // Velocity; return velocity * (t- t0)
            rhs_coefficient = 1.0f;
            var result = new MassPoint(m_ControlPoints[index].velocity, m_ControlPoints[index].mass) * (t - t0);
            result.mass = 0.0f;
            return result;
        }

        private MassPoint TestAcceleration(ref float rhs_coefficient, float t, float t0, float t1, float deno, int index, List<int> indices)
        {
            // Is acceleration
            if (indices.Count == 3 && indices.All(v => v == indices.First()))
            {
                // (t-t0)^2
                float coefficient = t - t0;
                coefficient *= coefficient;

                // Coefficient for point is 1
                rhs_coefficient = 1.0f;

                var result = new MassPoint(m_ControlPoints[index].acceleration, m_ControlPoints[index].mass) * (coefficient / 2.0f);
                result.mass = 0.0f;
                return result;
            }

            //  Perform calculation as normal
            return Run(t, indices.GetRange(0, indices.Count - 1)) * ((t1 - t) / deno);
        }
        public MassPoint Run(float t)
        {
            return Run(t, m_Indices);
        }

        private MassPoint Run(float t, List<int> indices)
        {
            // base case
            if (indices.Count == 2)
            {
                int _t0 = indices[0];
                int _t1 = indices[1];
                float _deno = _t1 - _t0;
                float _rhs_coefficient = (t - _t0) / _deno;

                // 2 points; Check if using velocity
                MassPoint _lhs = TestVelocity(ref _rhs_coefficient, t, _t0, _t1, _deno, _t0, indices);
                MassPoint _rhs = m_ControlPoints[_t1] * _rhs_coefficient;

                return _lhs + _rhs;
            }

            int t0 = indices.First();
            int t1 = indices.Last();
            int deno = (t1 - t0);
            float rhs_coefficient = (t - t0) / deno;    

            // More than 2 points; Check if using acceleration
            MassPoint lhs = TestAcceleration(ref rhs_coefficient, t, t0, t1, deno, t0, indices);
            MassPoint rhs = Run(t, indices.GetRange(1, indices.Count - 1)) * rhs_coefficient;

            return lhs + rhs;
        }
    }
}
