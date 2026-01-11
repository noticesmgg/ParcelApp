import React from 'react';
import './Loading.css';

const CenteredLoading: React.FC = () => {
  return (
    <div className="loading-overlay">
      <div className="loading-dots">
        <div></div><div></div><div></div>
      </div>
      <p className="loading-text">Loading...</p>
    </div>
  );
};

export default CenteredLoading;
