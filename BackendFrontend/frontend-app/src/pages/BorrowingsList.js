import React, { useEffect, useState } from 'react';
import { fetchBorrowings } from '../api/Services';
import '../styles/BorrowerManagement.css';

function BorrowingsList({ onNavigate }) {
  const [borrowings, setBorrowings] = useState([]);
  const [filteredBorrowings, setFilteredBorrowings] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [sortConfig, setSortConfig] = useState({ key: 'id', direction: 'asc' });
  const [currentPage, setCurrentPage] = useState(1);
  const [itemsPerPage] = useState(15);  // Change this to 15 rows per page
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchBorrowings()
      .then(data => {
        setBorrowings(data);
        setFilteredBorrowings(data);
      })
      .catch(err => {
        console.error('Failed to fetch borrowings:', err);
        setError('Could not fetch borrowings.');
      });
  }, []);

  const handleSearch = (e) => {
    const term = e.target.value;
    setSearchTerm(term);
    if (term === '') {
      setFilteredBorrowings(borrowings);
    } else {
      setFilteredBorrowings(
        borrowings.filter(b =>
          b.borrowerId.toString().includes(term) || 
          b.branchId.toString().includes(term) || 
          (b.borrowDate && b.borrowDate.includes(term)) || 
          (b.dueDate && b.dueDate.includes(term))
        )
      );
    }
  };

  const handleSort = (key) => {
    let direction = 'asc';
    if (sortConfig.key === key && sortConfig.direction === 'asc') {
      direction = 'desc';
    }
    setSortConfig({ key, direction });

    const sorted = [...filteredBorrowings].sort((a, b) => {
      if (a[key] < b[key]) return direction === 'asc' ? -1 : 1;
      if (a[key] > b[key]) return direction === 'asc' ? 1 : -1;
      return 0;
    });

    setFilteredBorrowings(sorted);
  };

  const indexOfLastItem = currentPage * itemsPerPage;
  const indexOfFirstItem = indexOfLastItem - itemsPerPage;
  const currentBorrowings = filteredBorrowings.slice(indexOfFirstItem, indexOfLastItem);

  const paginate = (pageNum) => setCurrentPage(pageNum);

  return (
    <div>
      <button className="back-button" onClick={() => onNavigate('list')}>← Back</button>
      <input
        type="text"
        placeholder="Search by borrower ID, book ID, or date..."
        value={searchTerm}
        onChange={handleSearch}
        className="search-input"
      />
      {error && <p className="error">{error}</p>}
      <table>
        <thead>
          <tr>
            <th onClick={() => handleSort('id')}>Borrowing ID</th>
            <th onClick={() => handleSort('borrowerId')}>Borrower</th>
            <th onClick={() => handleSort('branchId')}>Book Title</th>
            <th onClick={() => handleSort('borrowDate')}>Date Borrowed</th>
            <th onClick={() => handleSort('dueDate')}>Due Date</th>
            <th onClick={() => handleSort('returnStatus')}>Status</th>
          </tr>
        </thead>
        <tbody>
          {currentBorrowings.length > 0 ? (
            currentBorrowings.map(b => (
              <tr key={b.id}>
                <td>{b.id}</td>
                <td>{b.borrowerId}</td>
                <td>{b.branchId}</td>
                <td>{b.borrowDate}</td>
                <td>{b.dueDate}</td>
                <td>{b.returnStatus ? 'Returned' : 'Not Returned'}</td>
              </tr>
            ))
          ) : (
            <tr><td colSpan="6">No borrowings found.</td></tr>
          )}
        </tbody>
      </table>
      <div className="pagination">
        {Array.from({ length: Math.ceil(filteredBorrowings.length / itemsPerPage) }, (_, i) => (
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
  );
}

export default BorrowingsList;
