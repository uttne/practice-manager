import HashObjectStore from "./HashObjectStore";
import PracticeManagerApiClient from "./PracticeManagerApiClient";
import ScoreClient from "./ScoreClient";
import ScoreClientMock from "./ScoreClientMock";

export const apiClient = new PracticeManagerApiClient(
  process.env.REACT_APP_API_URI_BASE as string
);

export const hashObjectStore = new HashObjectStore(apiClient);

// export const scoreClient = new ScoreClient(apiClient, hashObjectStore);

export const scoreClient = new ScoreClientMock(apiClient, hashObjectStore);