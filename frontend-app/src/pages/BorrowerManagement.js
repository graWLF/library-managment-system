import React, { useState, useEffect } from 'react';
import { fetchBorrowers } from '../api/Services';
import AddBorrowerForm from './AddBorrowerForm';
import LendBookForm from './LendBookForm';
import '../styles/BorrowerManagement.css';
import BorrowingsList from './BorrowingsList';

function BorrowerManagement() {
  const [view, setView] = useState('list');
  const [borrowers, setBorrowers] = useState([]);
  const [filteredBorrowers, setFilteredBorrowers] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [sortConfig, setSortConfig] = useState({ key: 'id', direction: 'asc' });
  const [currentPage, setCurrentPage] = useState(1);
  const [itemsPerPage] = useState(10);
  const [error, setError] = useState(null);

  // Fetch borrowers on mount
  useEffect(() => {
    fetchBorrowers()
      .then(data => {
        setBorrowers(data);
        setFilteredBorrowers(data);
      })
      .catch(err => {
        console.error('Failed to fetch borrowers:', err);
        setError('Could not fetch borrowers.');
      });
  }, []);

  // Search handler
  const handleSearch = (e) => {
    const term = e.target.value;
    setSearchTerm(term);
    if (term === '') {
      setFilteredBorrowers(borrowers);
    } else {
      setFilteredBorrowers(
        borrowers.filter(b =>
          b.borrowerName.toLowerCase().includes(term.toLowerCase())
        )
      );
    }
  };

  // Sort handler
  const handleSort = (key) => {
    let direction = 'asc';
    if (sortConfig.key === key && sortConfig.direction === 'asc') {
      direction = 'desc';
    }
    setSortConfig({ key, direction });

    const sorted = [...filteredBorrowers].sort((a, b) => {
      if (a[key] < b[key]) return direction === 'asc' ? -1 : 1;
      if (a[key] > b[key]) return direction === 'asc' ? 1 : -1;
      return 0;
    });

    setFilteredBorrowers(sorted);
  };

  const indexOfLastItem = currentPage * itemsPerPage;
  const indexOfFirstItem = indexOfLastItem - itemsPerPage;
  const currentBorrowers = filteredBorrowers.slice(indexOfFirstItem, indexOfLastItem);

  const paginate = (pageNum) => setCurrentPage(pageNum);

  const handleSuccess = () => {
    setView('list');
    // Refetch list after add/lend
    fetchBorrowers().then(data => {
      setBorrowers(data);
      setFilteredBorrowers(data);
    });
  };

  return (
    <div className="borrower-management">
      <h2>Borrower Management</h2>

      {/* View buttons */}
      <div className="view-buttons">
        <button onClick={() => setView('list')}>See Borrower List</button>
        <button onClick={() => setView('add')}>Add a Borrower</button>
        <button onClick={() => setView('lend')}>Lend a Book</button>
        <button onClick={() => setView('borrowings')}>See Borrowings List</button>
      </div>  


      {/* View: List */}
      {view === 'list' && (
        <div>
          <input
            type="text"
            placeholder="Search by name..."
            value={searchTerm}
            onChange={handleSearch}
            className="search-input"
          />
          {error && <p className="error">{error}</p>}
          <table>
            <thead>
              <tr>
                <th onClick={() => handleSort('id')}>ID</th>
                <th onClick={() => handleSort('borrowerName')}>Name</th>
                <th onClick={() => handleSort('borrowerPhone')}>Phone</th>
              </tr>
            </thead>
            <tbody>
              {currentBorrowers.length > 0 ? (
                currentBorrowers.map(b => (
                  <tr key={b.id}>
                    <td>{b.id}</td>
                    <td>{b.borrowerName}</td>
                    <td>{b.borrowerPhone}</td>
                  </tr>
                ))
              ) : (
                <tr><td colSpan="3">No borrowers found.</td></tr>
              )}
            </tbody>
          </table>
          {/* Pagination */}
          <div className="pagination">
            {Array.from({ length: Math.ceil(filteredBorrowers.length / itemsPerPage) }, (_, i) => (
              <button
                key={i}
                onClick={() => paginate(i + 1)}
                className={currentPage === i + 1 ? 'active' : ''}
              >
                {i + 1}
              </button>
            ))}
          </div>
        </div>
      )}

      {/* View: Add */}
      {view === 'add' && (
        <div>
          <button className="back-button" onClick={() => setView('list')}>← Back</button>
          <AddBorrowerForm onSuccess={handleSuccess} />
        </div>
      )}

      {/* View: Lend */}
      {view === 'lend' && (
        <div>
          <button className="back-button" onClick={() => setView('list')}>← Back</button>
          <LendBookForm onSuccess={handleSuccess} />
        </div>
      )}
      {view === 'borrowings' && (
        <BorrowingsList onNavigate={setView} />
      )}


    </div>
  );
}

export default BorrowerManagement;
