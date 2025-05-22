import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { fetchBorrowerById, updateBorrower } from '../api/Services';

function EditBorrower() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [borrower, setBorrower] = useState({ borrowerName: '', borrowerPhone: '' });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    fetchBorrowerById(id)
      .then(data => {
        setBorrower(data);
        setLoading(false);
      })
      .catch(() => {
        setError('Failed to load borrower data.');
        setLoading(false);
      });
  }, [id]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setBorrower(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await updateBorrower(id, borrower);
      navigate('/admin/borrowers'); // or wherever the borrower list lives
    } catch (err) {
      setError('Failed to update borrower.');
      navigate('/admin/borrowers');
    }
  };

  if (loading) return <p>Loading...</p>;
//   if (error) return <p className="error">{error}</p>;

  return (
    <div className="edit-borrower-form">
      <h2>Edit Borrower</h2>
      <form onSubmit={handleSubmit}>
        <label>
          Name:
          <input
            type="text"
            name="borrowerName"
            value={borrower.borrowerName}
            onChange={handleChange}
            required
          />
        </label>
        <label>
          Phone:
          <input
            type="text"
            name="borrowerPhone"
            value={borrower.borrowerPhone}
            onChange={handleChange}
            required
          />
        </label>
        <button type="submit">Update</button>
        <button type="button" onClick={() => navigate(-1)}>Cancel</button>
      </form>
    </div>
  );
}

export default EditBorrower;
