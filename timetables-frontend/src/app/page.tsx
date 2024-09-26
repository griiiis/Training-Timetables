"use client";
import ContestTable from "@/components/Search";
import { IContest } from "@/domain/IContest";
import { IUserContestPackage } from "@/domain/IUserContestPackage";
import ContestService from "@/services/ContestService";
import UserContestPackageService from "@/services/UserContestPackageService";
import Link from "next/link";
import React, { useLayoutEffect } from "react";
import { useEffect, useState } from "react";

export default function Contest() {
  const [isLoading, setIsLoading] = useState(true);
  const [allUserContestPackages, setAllUserContestPackages] = useState<IUserContestPackage[]>([]);
  const [currentUserPackages, setCurrentUserPackages] = useState<IUserContestPackage[]>([]);

  const [currentContests, setCurrentContests] = useState<IContest[]>([]);
  const [comingContests, setComingContests] = useState<IContest[]>([]);

  const [searchedContests, setSearchedContests] = useState<IContest[]>([]);
  const [allContests, setAllContests] = useState<IContest[]>([]);

  let [searchInput, setSearchInput] = useState("");
  const [locationInput, setLocationInput] = useState<boolean>(false);
  const [contestTypeInput, setContestTypeInput] = useState<boolean>(false);
  const [gameTypeInput, setGameTypeInput] = useState<boolean>(false);

  const loadData = async () => {
    const contestResponse = await ContestService.getAll();
    const allUserPackages = await UserContestPackageService.getAll();

    console.log(contestResponse.data)

    if (localStorage.getItem("userInfo") !== null) {
      const userContestPackagesResponse =
        await UserContestPackageService.getCurrentUserPackages();
      setCurrentUserPackages(userContestPackagesResponse.data!);
    }

    if (contestResponse.data && allUserPackages.data) {

      const currentContests = contestResponse.data
        .filter((contest) => {
          const fromDate = new Date(contest.from);
          const untilDate = new Date(contest.until);
          const now = new Date();
          return fromDate < now && untilDate > now;
        })
        .slice(0, 2);

      setCurrentContests(currentContests);

      const comingContests = contestResponse.data.filter((contest) => {
        const fromDate = new Date(contest.from);
        const now = new Date();
        return fromDate > now;
      });
      setComingContests(comingContests);
      setAllUserContestPackages(allUserPackages.data);
      setAllContests(contestResponse.data);

      setSearchedContests(contestResponse.data.slice(0,3))

      setIsLoading(false);
    }
  };

  const SearchData = () => {
    let searchedContests = allContests;

      if(searchInput !== ""){
        
        searchInput = searchInput.toUpperCase();

        if(contestTypeInput){
          searchedContests = searchedContests.filter(e => e.contestType.contestTypeName.toUpperCase().includes(searchInput))
        }
        if(locationInput){
          searchedContests = searchedContests.filter(e => e.location.locationName.toUpperCase().includes(searchInput))
        }
        if(gameTypeInput){
          searchedContests = searchedContests.filter(e => e.contestGameTypes.some(e => e.gameType.gameTypeName.toUpperCase().includes(searchInput)));
        }
      }
      setSearchedContests(searchedContests)
    
  }
  useEffect(() => {
    loadData();
  }, []);
 
  if (isLoading) return <h1 className="middle"> Contests - Loading</h1>;

  return (
    <>
      <h1 className="middle">Contests</h1>
      <br />

      {currentContests.length > 0 && (
        <div className="ended-contests">
          <h2 className="section-title">Current Contests</h2>
          <div className="row">
            {currentContests.map((contest) => {
              return (
                <React.Fragment key={`${contest.id}`}>
                  <div className="col-md-6 mb-4">
                    <div className="card ended-contest-card">
                      <div className="card-body">
                        <div className="row">
                          <div className="col-md-10">
                            <h2 className="contest-name">
                              {contest.contestName}
                            </h2>
                            <p className="contest-duration">
                              {new Date(contest.from).toDateString() +
                                " " +
                                new Date(contest.from)
                                  .toTimeString()
                                  .substring(0, 8)}
                              -{" "}
                              {new Date(contest.until).toDateString() +
                                " " +
                                new Date(contest.until)
                                  .toTimeString()
                                  .substring(0, 8)}
                            </p>
                            <p className="contest-detail">
                              <strong>Location:</strong>{" "}
                              {contest.location.locationName}
                            </p>
                            <p className="contest-detail">
                              <strong>Total Hours:</strong> {contest.totalHours}
                            </p>
                            <p className="contest-detail">
                              <strong>Contest Type:</strong>{" "}
                              {contest.contestType.contestTypeName}
                            </p>
                            <p className="contest-detail">
                              <strong>Number of Participants:</strong>{" "}
                              {
                                allUserContestPackages.filter(
                                  (e) => e.contestId === contest.id
                                ).length
                              }
                            </p>
                            <p className="contest-detail">
                              <strong>Game Types:</strong>{" "}
                              {contest.contestGameTypes.map(
                                (gameType, index) => (
                                  <span key={index}>
                                    {gameType.gameType.gameTypeName}
                                    {index !==
                                    contest.contestGameTypes.length - 1
                                      ? ", "
                                      : ""}
                                  </span>
                                )
                              )}
                              ;
                            </p>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </React.Fragment>
              );
            })}
          </div>
        </div>
      )}

      {comingContests.length > 0 && (
        <div className="ended-contests">
          <h2 className="middle">Coming Contests</h2>
          <div className="row">
            {comingContests.map((contest) => {
              return (
                <React.Fragment key={`${contest.id}`}>
                  <div className="col-md-6 mb-4">
                    <div className="card ended-contest-card">
                      <div className="card-body">
                        <div className="row">
                          <div className="col-md-10">
                            <h2 className="contest-name">
                              {contest.contestName}
                            </h2>
                            <p className="contest-duration">
                              {new Date(contest.from).toDateString() +
                                " " +
                                new Date(contest.from)
                                  .toTimeString()
                                  .substring(0, 8)}
                              -{" "}
                              {new Date(contest.until).toDateString() +
                                " " +
                                new Date(contest.until)
                                  .toTimeString()
                                  .substring(0, 8)}
                            </p>
                            <p className="contest-detail">
                              <strong>Location:</strong>{" "}
                              {contest.location.locationName}
                            </p>
                            <p className="contest-detail">
                              <strong>Total Hours:</strong> {contest.totalHours}
                            </p>
                            <p className="contest-detail">
                              <strong>Contest Type:</strong>{" "}
                              {contest.contestType.contestTypeName}
                            </p>
                            <p className="contest-detail">
                              <strong>Number of Participants:</strong>{" "}
                              {
                                allUserContestPackages.filter(
                                  (e) => e.contestId === contest.id
                                ).length
                              }
                            </p>
                            <p className="contest-detail">
                              <strong>Game Types:</strong>{" "}
                              {contest.contestGameTypes.map(
                                (gameType, index) => (
                                  <span key={index}>
                                    {gameType.gameType.gameTypeName}
                                    {index !==
                                    contest.contestGameTypes.length - 1
                                      ? ", "
                                      : ""}
                                  </span>
                                )
                              )}
                              ;
                            </p>
                            {localStorage.getItem("userInfo") !== null &&
                            currentUserPackages !== undefined &&
                            currentUserPackages.filter(
                              (e) => e.contestId === contest.id
                            ).length > 0 ? (
                              <div className="contest-actions">
                                <button className="btn btn-success" disabled>
                                  Already Joined
                                </button>
                              </div>
                            ) : (
                              <div className="contest-actions">
                                {localStorage.getItem("userInfo") === null ? (
                                  <Link
                                    className="btn btn-success"
                                    href={`/Account/Login`}
                                  >
                                    Join The Contest!
                                  </Link>
                                ) : (
                                  <Link
                                    className="btn btn-success"
                                    href={`/UserContestPackage/Create/${contest.id}`}
                                  >
                                    Join The Contest!
                                  </Link>
                                )}
                              </div>
                            )}
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </React.Fragment>
              );
            })}
          </div>
        </div>
      )}

<form>
        <div className="mb-3">
          <label className="control-label" htmlFor="Search">
            Search 
          </label>
          <input
            className="form-control"
            type="text"
            id="Search"
            value={searchInput}
            onChange={(e) => {
              setSearchInput(e.target.value);
            }}
          />
        </div>
        <div className="mb-3 p-2 and">
          <input
            className="form-check-input"
            type="checkbox"
            id="Location"
            checked={locationInput !== undefined ? locationInput : false}
            onChange={(e) => {
              setLocationInput(e.target.checked);
            }}
          />
          <label className="control-label" htmlFor="Location"> Location </label>
        </div>
        <div className="mb-3 p-2 and">
          <input
            className="form-check-input"
            type="checkbox"
            id="ContestType"
            checked={contestTypeInput !== undefined ? contestTypeInput : false}
            onChange={(e) => {
              setContestTypeInput(e.target.checked);
            }}
          />
          <label className="control-label" htmlFor="ContestType">
            ContestType
          </label>
        </div>
        <div className="mb-3 p-2 and">
          <input
            className="form-check-input"
            type="checkbox"
            id="GameType"
            checked={gameTypeInput !== undefined ? gameTypeInput : false}
            onChange={(e) => {
              setGameTypeInput(e.target.checked);
            }}
          />
          <label className="control-label" htmlFor="GameType">
            GameType
          </label>
        </div>
        <button onClick={(e) => { SearchData(), e.preventDefault(); }} type="submit" className="btn btn-primary">Search</button>
      </form>

      <ContestTable searchedContests={searchedContests} />
    </>
  );
}

