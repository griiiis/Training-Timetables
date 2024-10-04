import { IInformationContestDTO } from '@/domain/DTOs/Contests/IInformationContestDTO';
import { IUserInfo } from '@/domain/Identity/IUserInfo';
import { ILevel } from '@/domain/ILevel';
import { IPackageGameTypeTime } from '@/domain/IPackageGameTypeTime';
import ContestService from '@/services/ContestService';
import LevelService from '@/services/LevelService';
import PackageGameTypeTimeService from '@/services/PackageGameTypeTimeService';
import UserContestPackageService from '@/services/UserContestPackageService';
import { useRouter } from 'next/navigation';
import { useEffect, useState } from 'react';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';

type Props = {
  contestId: string;
};

function JoinContestModal(props: Props) {
  const [show, setShow] = useState(false);

  const handleClose = () => setShow(false);
  const handleShow = () => setShow(true);

  const router = useRouter();
  const [contest, setContest] = useState<IInformationContestDTO>();
  const [packages, setPackages] = useState<IPackageGameTypeTime[]>([]);
  const [levels, setLevels] = useState<ILevel[]>([]);
  const [selectedPackageId, setSelectedPackageId] = useState("");
  const [levelId, setLevelId] = useState("");
  const [isLoading, setIsLoading] = useState(true);

  const CreateNewUserPackage = async () => {
      const UserContestPackageData = {
          packageGameTypeTimeId: selectedPackageId,
          hoursAvailable: contest!.totalHours,
          contestId: contest!.id,
          levelId: levelId
      }
      const response = await UserContestPackageService.postUserContestPackage(UserContestPackageData);
      if (response.data) {
          let userInfo: IUserInfo = JSON.parse(localStorage.getItem("userInfo")!);
          userInfo.role = "Participant"
          localStorage.setItem("userInfo", JSON.stringify(userInfo));
          window.dispatchEvent(new Event("storage"));
          router.push("/MyContests");
      }
  };

  
  const loadData = async () => {
      const contestResponse = await ContestService.getContestInformation(props.contestId.toString());
      const packagesResponse = await PackageGameTypeTimeService.getCurrentContestPackages(props.contestId.toString());
      const levelsResponse = await LevelService.getCurrentContestLevels(props.contestId.toString());
      if (contestResponse.data && packagesResponse.data && levelsResponse.data) {
          setContest(contestResponse.data);
          setPackages(packagesResponse.data);
          setLevels(levelsResponse.data);
          setIsLoading(false);
      };
  }
  useEffect(() => { loadData() }, []);

  if (isLoading) return (<h1 className="success">...</h1>)


  return (
    <>
      <Button variant="success" onClick={handleShow}>
        Join the Contest
      </Button>

      <Modal show={show} onHide={handleClose}>
        <Modal.Header closeButton>
          <Modal.Title>Join the Contest</Modal.Title>
        </Modal.Header>
        <Modal.Body>
        <form>
                <div className="mb-3 row">
                    <div className="col-md-4">
                        <div className="form-group">
                            <label className="control-label" htmlFor="Packages">Packages</label>
                            <select className="form-control" onChange={(e) => setSelectedPackageId(e.target.value)}><option>Please choose one option</option>{packages.map((packages) => {
                                return (
                                    <option key={packages.id} value={packages.id}>
                                        {packages.packageGtName}
                                    </option>
                                );
                            })}</select>
                        </div>
                        <div className="form-group">
                            <label className="control-label" htmlFor="Levels">Levels</label>
                            <select className="form-control" onChange={(e) => setLevelId(e.target.value)}><option>Please choose one option</option>{levels.map((level) => {
                                return (
                                    <option key={level.id} value={level.id}>
                                        {level.title}
                                    </option>
                                );
                            })}</select>
                        </div>
                    </div>
                </div>
            </form>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={handleClose}>
            Close
          </Button>
          <Button variant="primary" onClick={(e) => { CreateNewUserPackage(), e.preventDefault(); }} type="submit">
          Submit
          </Button>
        </Modal.Footer>
      </Modal>
    </>
  );
}

export default JoinContestModal;