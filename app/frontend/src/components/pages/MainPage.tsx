import { createStyles, makeStyles, Theme } from "@material-ui/core";
import { useContext, useEffect } from "react";
import { BrowserRouter as Router, Route, Switch } from "react-router-dom";
import { AppContextDispatch } from "../../AppContext";
import { UserData } from "../../UserClient";
import ScoreDetailContent from "../organisms/ScoreDetailContent";
import ScoreSummaryListContent from "../organisms/ScoreSummaryListContent";
import ScoreNewContent from "../organisms/ScoreNewContent";
import ScorePageEditContent from "../organisms/ScorePageEditContent";
import MainTemplate from "../templates/MainTemplate";

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      height: "100%",
      width: "100%",
      display: "flex",
      flexWrap: "wrap",
    },
    navi: {
      height: "100px",
      width: "100%",
    },
    content: {},
  })
);

export interface MainPageProps {
  userData?: UserData;
}

export default function MainPage(props: MainPageProps) {
  const _userData = props.userData;
  const classes = useStyles();
  const dispatch = useContext(AppContextDispatch);

  useEffect(() => {
    console.log("_userData");
    console.log(_userData);
    dispatch({ type: "updateUserData", payload: _userData });
  }, [_userData]);

  return (
    <MainTemplate>
      <div className={classes.root}>
        <Router>
          <Switch>
            <Route path="/scores/new" component={ScoreNewContent} />
            <Route
              path="/scores/:scoreId/edit-page"
              component={ScorePageEditContent}
            />
            <Route
              path={[
                "/scores/:scoreId/page/:pageId",
                "/scores/:scoreId/snapshot/:snapshotId/page/:snapshotPageId",
                "/scores/:scoreId/snapshot/:snapshotId",
                "/scores/:scoreId",
              ]}
              component={ScoreDetailContent}
            />
            <Route
              path={["/scores", "/"]}
              component={ScoreSummaryListContent}
            />
          </Switch>
        </Router>
      </div>
    </MainTemplate>
  );
}
