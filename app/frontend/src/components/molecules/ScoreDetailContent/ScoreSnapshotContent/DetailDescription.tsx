import { createStyles, makeStyles, Theme } from "@material-ui/core";

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      width: "100%",
    },
    descriptionContainer: {
      width: "100%",
    },
    descriptionTitle: {
      height: "30px",
      width: "100%",
      display: "flex",
      alignItems: "center",
      "& > p": {
        margin: 0,
      },
    },
    description: {
      width: "100%",
      "& > p": {
        margin: "2px 0",
      },
    },
  })
);

export interface DetailDescriptionProps {
  description?: string;
}

export default function DetailDescription(props: DetailDescriptionProps) {
  const _description = props.description;
  const classes = useStyles();

  return (
    <div className={classes.root}>
      <div className={classes.descriptionContainer}>
        <div className={classes.descriptionTitle}>
          <p>説明</p>
        </div>
        <div className={classes.description}>
          {_description?.split("\n").map((p, index) => (
            <p key={index}>{p}</p>
          ))}
        </div>
      </div>
    </div>
  );
}
