import { IContest } from '@/domain/IContest';
import React, { useState, useEffect } from 'react';

type SortKey = 'contestName' | 'from' | 'until' | 'totalHours' | 'contestType' | 'location';
type Direction = 'ascending' | 'descending';

interface SortConfig {
  key: SortKey;
  direction: Direction;
}

const ContestTable = ({ searchedContests } : {searchedContests: IContest[]}) => {

  const [sortedContests, setSortedContests] = useState<IContest[]>(searchedContests);
  const [sortConfig, setSortConfig] = useState<SortConfig>({ key: 'contestName', direction: 'ascending' });

  const sortArray = (array: IContest[], key: SortKey, direction: Direction): IContest[] => {
    return array.sort((a, b) => {
      const aValue = a[key];
      const bValue = b[key];

      if (aValue < bValue) {
        return direction === 'ascending' ? -1 : 1;
      }
      if (aValue > bValue) {
        return direction === 'ascending' ? 1 : -1;
      }
      return 0;
    });
  };

  useEffect(() => {
    let sortedArray = [...searchedContests];
    sortedArray = sortArray(sortedArray, sortConfig.key, sortConfig.direction);
    setSortedContests(sortedArray);
  }, [searchedContests, sortConfig]);

  const handleSort = (key: SortKey) => {
    let direction: 'ascending' | 'descending' = 'ascending';
    if (sortConfig.key === key && sortConfig.direction === 'ascending') {
      direction = 'descending';
    }
    setSortConfig({ key, direction });
  };

  return (
    <div className="container">
      <table className="table table-hover">
        <thead className="thead-dark">
          <tr>
            <th scope="col" onClick={() => handleSort('contestName')}> Name {sortConfig.key === 'contestName' && <span>{sortConfig.direction === 'ascending' ? '▲' : '▼'}</span>}</th>
            <th scope="col" onClick={() => handleSort('from')}>From {sortConfig.key === 'from' && <span>{sortConfig.direction === 'ascending' ? '▲' : '▼'}</span>}</th>
            <th scope="col" onClick={() => handleSort('until')}>Until {sortConfig.key === 'until' && <span>{sortConfig.direction === 'ascending' ? '▲' : '▼'}</span>}</th>
            <th scope="col" onClick={() => handleSort('totalHours')}>Total Hours {sortConfig.key === 'totalHours' && <span>{sortConfig.direction === 'ascending' ? '▲' : '▼'}</span>}</th>
            <th scope="col" onClick={() => handleSort('contestType')}>Type {sortConfig.key === 'contestType' && <span>{sortConfig.direction === 'ascending' ? '▲' : '▼'}</span>}</th>
            <th scope="col" onClick={() => handleSort('location')}>Location Name {sortConfig.key === 'location' && <span>{sortConfig.direction === 'ascending' ? '▲' : '▼'}</span>}</th>
            <th scope="col"></th>
          </tr>
        </thead>
        <tbody>
          {sortedContests.map((contest) => (
            <React.Fragment key={contest.id}>
              <tr>
                <td>{contest.contestName}</td>
                <td>{contest.from.substring(0, 10)}</td>
                <td>{contest.until.substring(0, 10)}</td>
                <td>{contest.totalHours}</td>
                <td>{contest.contestType.contestTypeName}</td>
                <td>{contest.location.locationName}</td>
              </tr>
            </React.Fragment>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default ContestTable;
