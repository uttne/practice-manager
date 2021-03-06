/** 楽譜のサマリー情報 */
export interface ScoreSummary {
  id: string;
  owner_id: string;
  title: string;
  description: string;
}

/** 新しい楽譜 */
export interface NewScore {
  title: string;
  description?: string;
}

/** 新しいく作られた楽譜 */
export interface NewlyScore {
  id: string;
}

/** 楽譜のページ */
export interface ScorePage {
  id: number;
  itemId: string;
  page: string;
  objectName: string;
}

/** 楽譜のアノテーション */
export interface ScoreAnnotation {
  id: string;
  contentHash: string;
}

/** 楽譜のデータ */
export interface ScoreData {
  title: string;
  descriptionHash: string;
  pages: ScorePage[];
  annotations: ScoreAnnotation[];
}

/** 新しいタイトル */
export interface NewScoreTitle {
  title: string;
}

/** 新しい説明 */
export interface NewScoreDescription {
  description: string;
}

export interface ScoreSnapshotSummary {
  id: string;
  name: string;
  createAt: Date;
}

export interface NewScoreSnapshot {
  name: string;
}

export interface ScoreSnapshotDetail {
  id: string;
  name: string;
  data: ScoreData;
  hashSet: { [hash: string]: string };
}
/** 新しくアップロードされた楽譜のアイテム */
export interface NewlyScoreItem {
  itemInfo: {
    originalName: string;
    thumbnail: string;
    thumbnailSize: number;
    scoreId: string;
    itemId: string;
    objectName: string;
    size: number;
    totalSize: number;
  };
}

export interface NewScorePage {
  itemId: string;
  objectName: string;
  page: string;
}

export interface PatchScorePage {
  targetPageId: number;
  itemId: string;
  page: string;
  objectName: string;
}

//--------------------------------------------------------------------

/** アクセスのタイプ */
type Accesses = "private" | "public";

/** 楽譜の詳細 */
export interface ScoreDetail {
  createAt: Date;
  updateAt: Date;
  data: ScoreData;
  dataHash: string;
  access: Accesses;
  hashSet: { [hash: string]: string };
}

//-------------------------------------------------------------------------------

const GET_HEADERS = {
  "Content-Type": "application/json",
};

const POST_HEADERS = {
  "Content-Type": "application/json",
};

const PATCH_HEADERS = {
  "Content-Type": "application/json",
};

const DELETE_HEADERS = {
  "Content-Type": "application/json",
};
/** 初期化されていない */
const NotInitializedScore = 520;

/** 楽譜がない */
const NotFoundScore = 521;

/** 楽譜に関するリクエストを実行するクライアント */
export default class ScoreClientV2 {
  constructor(private baseUrl: string) {
    let i = baseUrl.length;
    for (; 1 <= i; --i) {
      if (baseUrl[i - 1] === "/") {
        continue;
      }
      break;
    }

    if (baseUrl.length !== i) {
      this.baseUrl = baseUrl.substr(0, i);
    }
  }

  /** 自分の楽譜のサマリーを一覧で取得する */
  async getMyScoreSummaries(): Promise<ScoreSummary[]> {
    const requestUrl = this.baseUrl + "/scores/user";

    try {
      const response = await fetch(requestUrl, {
        method: "GET",
        headers: GET_HEADERS,
        credentials: "include",
      });

      const json = await response.json();
      return json;
    } catch (err) {
      throw err;
    }
  }

  /** 楽譜を作成する */
  async create(newScore: NewScore): Promise<NewlyScore> {
    const requestUrl = this.baseUrl + "/scores/user";

    try {
      const response = await fetch(requestUrl, {
        method: "POST",
        headers: POST_HEADERS,
        credentials: "include",
        body: JSON.stringify(newScore),
      });

      if (response.ok) {
        return (await response.json()) as NewlyScore;
      } else if (response.status === NotInitializedScore) {
        const initRequestUrl = this.baseUrl + "/scores/new";
        const initResponse = await fetch(initRequestUrl, {
          method: "POST",
          headers: POST_HEADERS,
          credentials: "include",
        });

        if (initResponse.ok) {
          const response2 = await fetch(requestUrl, {
            method: "POST",
            headers: POST_HEADERS,
            credentials: "include",
            body: JSON.stringify(newScore),
          });

          if (response2.ok) {
            return (await response2.json()) as NewlyScore;
          }
        }
      }
      throw new Error("楽譜の作成に失敗");
    } catch (err) {
      throw err;
    }
  }

  async getDetail(scoreId: string): Promise<ScoreDetail | undefined> {
    const requestUrl = this.baseUrl + `/scores/user/${scoreId}`;

    try {
      const response = await fetch(requestUrl, {
        method: "GET",
        headers: GET_HEADERS,
        credentials: "include",
      });

      if (response.ok) {
        var json = await response.json();

        return json;
      } else if (response.status === NotFoundScore) {
        return undefined;
      } else if (response.status === 404) {
        return undefined;
      }
      throw new Error();
    } catch (err) {
      throw err;
    }
  }

  async updateTitle(scoreId: string, newTitle: NewScoreTitle): Promise<void> {
    const requestUrl = this.baseUrl + `/scores/user/${scoreId}/title`;

    try {
      const response = await fetch(requestUrl, {
        method: "PATCH",
        headers: PATCH_HEADERS,
        credentials: "include",
        body: JSON.stringify(newTitle),
      });

      if (response.ok) {
        return;
      }
      throw new Error("タイトルの更新に失敗");
    } catch (err) {
      throw err;
    }
  }

  async updateDescription(
    scoreId: string,
    newDescription: NewScoreDescription
  ): Promise<void> {
    const requestUrl = this.baseUrl + `/scores/user/${scoreId}/description`;

    try {
      const response = await fetch(requestUrl, {
        method: "PATCH",
        headers: PATCH_HEADERS,
        credentials: "include",
        body: JSON.stringify(newDescription),
      });

      if (response.ok) {
        return;
      }
      throw new Error("説明の更新に失敗");
    } catch (err) {
      throw err;
    }
  }

  async addPages(scoreId: string, newPages: NewScorePage[]): Promise<void> {
    const requestUrl = this.baseUrl + `/scores/user/${scoreId}/pages`;

    try {
      const response = await fetch(requestUrl, {
        method: "POST",
        headers: POST_HEADERS,
        credentials: "include",
        body: JSON.stringify(newPages),
      });

      if (response.ok) {
        return;
      }
      throw new Error("ページの追加に失敗");
    } catch (err) {
      throw err;
    }
  }

  async removePages(scoreId: string, ids: number[]): Promise<void> {
    const requestUrl = this.baseUrl + `/scores/user/${scoreId}/pages`;

    try {
      const response = await fetch(requestUrl, {
        method: "DELETE",
        headers: DELETE_HEADERS,
        credentials: "include",
        body: JSON.stringify(ids),
      });

      if (response.ok) {
        return;
      }
      throw new Error("ページの削除に失敗");
    } catch (err) {
      throw err;
    }
  }

  async updatePages(scoreId: string, pages: PatchScorePage[]): Promise<void> {
    const requestUrl = this.baseUrl + `/scores/user/${scoreId}/pages`;

    try {
      const response = await fetch(requestUrl, {
        method: "PATCH",
        headers: PATCH_HEADERS,
        credentials: "include",
        body: JSON.stringify(pages),
      });

      if (response.ok) {
        return;
      } else if (response.status === NotFoundScore) {
        throw new Error();
      } else if (response.status === 404) {
        throw new Error();
      }
      throw new Error("ページの更新に失敗");
    } catch (err) {
      throw err;
    }
  }

  async getSnapshotSummaries(scoreId: string): Promise<ScoreSnapshotSummary[]> {
    const requestUrl = this.baseUrl + `/scores/user/${scoreId}/snapshots`;

    try {
      const response = await fetch(requestUrl, {
        method: "GET",
        headers: GET_HEADERS,
        credentials: "include",
      });

      if (response.ok) {
        var json = await response.json();

        return json;
      } else if (response.status === NotFoundScore) {
        throw new Error();
      } else if (response.status === 404) {
        throw new Error();
      }
      throw new Error();
    } catch (err) {
      throw err;
    }
  }

  async createSnapshot(
    scoreId: string,
    newSnapshot: NewScoreSnapshot
  ): Promise<void> {
    const requestUrl = this.baseUrl + `/scores/user/${scoreId}/snapshots`;
    try {
      const response = await fetch(requestUrl, {
        method: "POST",
        headers: POST_HEADERS,
        credentials: "include",
        body: JSON.stringify(newSnapshot),
      });

      if (response.ok) {
        return;
      }
      throw new Error("スナップショットの作成に失敗しました");
    } catch (err) {
      throw err;
    }
  }

  async getSnapshotDetail(
    scoreId: string,
    snapshotId: string
  ): Promise<ScoreSnapshotDetail> {
    const requestUrl =
      this.baseUrl + `/scores/user/${scoreId}/snapshots/${snapshotId}`;

    try {
      const response = await fetch(requestUrl, {
        method: "GET",
        headers: GET_HEADERS,
        credentials: "include",
      });

      if (response.ok) {
        var json = await response.json();

        return json;
      } else if (response.status === NotFoundScore) {
        throw new Error();
      } else if (response.status === 404) {
        throw new Error();
      }
      throw new Error();
    } catch (err) {
      throw err;
    }
  }

  async deleteSnapshot(scoreId: string, snapshotId: string): Promise<void> {
    const requestUrl =
      this.baseUrl + `/scores/user/${scoreId}/snapshots/${snapshotId}`;

    try {
      const response = await fetch(requestUrl, {
        method: "DELETE",
        headers: DELETE_HEADERS,
        credentials: "include",
      });

      if (response.ok) {
        return;
      } else if (response.status === NotFoundScore) {
        throw new Error();
      } else if (response.status === 404) {
        throw new Error();
      }
      throw new Error();
    } catch (err) {
      throw err;
    }
  }

  async uploadItem(scoreId: string, item: File): Promise<NewlyScoreItem> {
    const url = this.baseUrl + `/items/user`;
    const formData = new FormData();
    formData.append("scoreId", scoreId);
    formData.append("item", item);
    formData.append("originalName", item.name);

    const response = await fetch(url, {
      method: "POST",
      body: formData,
      credentials: "include",
    });

    if (response.ok) {
      const json = await response.json();
      return json;
    }

    throw new Error(`アップロード失敗`);
  }
}
