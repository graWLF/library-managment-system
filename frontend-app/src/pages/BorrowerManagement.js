import React, { useState, useEffect } from 'react';
import { fetchBorrowers } from '../api/Services'; // Ensure this function fetches borrower data from the API
import '../styles/BorrowerManagement.css';

function BorrowerManagement() {
  const [borrowers, setBorrowers] = useState([]); // State to store borrower data
  const [error, setError] = useState(null); // State to handle errors

  // Fetch borrower data when the component loads
  useEffect(() => {
    fetchBorrowers()
      .then((data) => {
        setBorrowers(data); // Set the fetched data to the state
      })
      .catch((err) => {
        console.error('Error fetching borrowers:', err);
        setError('Failed to fetch borrower data.');
      });
  }, []);

  return (
    <div>
      <h2>Borrower Management</h2>
      <button>Add Borrower</button>
      {error && <p className="error">{error}</p>} {/* Display error if any */}
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Phone</th>
          </tr>
        </thead>
        <tbody>
          {borrowers.length > 0 ? (
            borrowers.map((borrower) => (
              <tr key={borrower.id}>
                <td>{borrower.id}</td>
                <td>{borrower.borrowerName}</td>
                <td>{borrower.borrowerPhone}</td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan="3">No borrowers found.</td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
}

export default BorrowerManagement;