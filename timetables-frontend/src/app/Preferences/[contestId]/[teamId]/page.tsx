"use client"

import { IInformationContestDTO } from "@/domain/DTOs/Contests/IInformationContestDTO";
import { IContest } from "@/domain/IContest";
import { ITimeOfDay } from "@/domain/ITimeOfDay";
import { ITimeTeam } from "@/domain/ITimeTeam";
import ContestService from "@/services/ContestService";
import TimeOfDayService from "@/services/TimeOfDayService";
import TimeTeamService from "@/services/TimeTeamService";
import { useParams } from "next/navigation"
import { useEffect, useState } from "react";




export default function Preferences() {

    let {teamId} = useParams();
    let {contestId} = useParams();
    const [teamTimes, setTeamTimes] = useState<ITimeTeam[]>([]);
    const [contest, setContest] = useState<IInformationContestDTO>();
    const [timeOfDays, setTimeOfDays] = useState<ITimeOfDay[]>([]);
    const [dates, setDates] = useState<Date[]>([]);
    
    const [isLoading, setIsLoading] = useState(true);


    const loadData = async () => {
        const currentTeamTimesResponse = await TimeTeamService.getCurrentTimeTeams(teamId.toString());
        const contestResponse = await ContestService.getContestInformation(contestId.toString());
        const allTimeOfDaysService = await TimeOfDayService.getContestTimeOfDays(contestId.toString());

        if (currentTeamTimesResponse.data && contestResponse.data && allTimeOfDaysService.data){
            setTeamTimes(currentTeamTimesResponse.data);
            setContest(contestResponse.data)
            setTimeOfDays(allTimeOfDaysService.data);

            const startDate = new Date(contestResponse.data.from);
            const endDate = new Date(contestResponse.data.until);
            endDate.setDate(endDate.getDate() + 1)

            const dates = [];
            let currentDate = new Date(startDate);
            while (currentDate <= endDate) {
              dates.push(new Date(currentDate));
              currentDate.setDate(currentDate.getDate() + 1);
            }
            setDates(dates);
            setIsLoading(false);
        }
    };

    useEffect(() => { loadData() }, []);
    

    if (isLoading) return (<h1>Preferences - LOADING</h1>)

        return (
            <div className="date-selection">
              {dates.map(date => {
                const dateString = date.toISOString().split('T')[0];
                return (
                  <div key={dateString} className="date-block">
                    <h3>{dateString}</h3>
                    <div className="time-selection">
                      {['morning', 'afternoon', 'evening'].map(time => (
                        <label key={time}>
                          {time.charAt(0).toUpperCase() + time.slice(1)}:
                          <input
                            type="checkbox"
                          />
                        </label>
                      ))}
                    </div>
                  </div>
                );
              })}
            </div>
          );
        
};