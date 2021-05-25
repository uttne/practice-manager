import * as cdk from '@aws-cdk/core';
import * as path from 'path';
import * as dotenv from 'dotenv';
import { ScoreHistoryBackendScoreDynamoDb } from './ScoreHistoryBackendScoreDynamoDb';
import { ScoreHistoryBackendScoreItemDynamoDb } from './ScoreHistoryBackendScoreItemDynamoDb';
import { ScoreHistoryBackendScoreLargeDataDynamoDb } from './ScoreHistoryBackendScoreLargeDataDynamoDb';
import { ScoreHistoryBackendScoreDataBucket } from './ScoreHistoryBackendScoreDataBucket';
import { ScoreHistoryBackendScoreDataSnapshotBucket } from './ScoreHistoryBackendScoreDataSnapshotBucket';

dotenv.config();

/** 楽譜データを格納する DynamoDB のテーブル */
const SCORE_DYNAMODB_TABLE_NAME = process.env
  .URA_KATA_SCORE_HISTORY_BACKEND_SCORE_DYNAMODB_TABLE_NAME as string;

if (!SCORE_DYNAMODB_TABLE_NAME) {
  throw new Error(
    "'URA_KATA_SCORE_HISTORY_BACKEND_SCORE_DYNAMODB_TABLE_NAME' is not found."
  );
}
/** 楽譜アイテムデータのメタ情報を格納する DynamoDB のテーブル */
const SCORE_ITEM_DYNAMODB_TABLE_NAME = process.env
  .URA_KATA_SCORE_HISTORY_BACKEND_SCORE_ITEM_DYNAMODB_TABLE_NAME as string;

if (!SCORE_DYNAMODB_TABLE_NAME) {
  throw new Error(
    "'URA_KATA_SCORE_HISTORY_BACKEND_SCORE_ITEM_DYNAMODB_TABLE_NAME' is not found."
  );
}
/** 楽譜データの大きいデータを格納する DynamoDB のテーブル */
const SCORE_LARGE_DATA_DYNAMODB_TABLE_NAME = process.env
  .URA_KATA_SCORE_HISTORY_BACKEND_SCORE_LARGE_DATA_DYNAMODB_TABLE_NAME as string;

if (!SCORE_DYNAMODB_TABLE_NAME) {
  throw new Error(
    "'URA_KATA_SCORE_HISTORY_BACKEND_SCORE_LARGE_DATA_DYNAMODB_TABLE_NAME' is not found."
  );
}
/** 楽譜のアイテムデータを格納する S3 バケット */
const SCORE_ITEM_S3_BUCKET = process.env
  .URA_KATA_SCORE_HISTORY_BACKEND_SCORE_ITEM_S3_BUCKET as string;

if (!SCORE_DYNAMODB_TABLE_NAME) {
  throw new Error(
    "'URA_KATA_SCORE_HISTORY_BACKEND_SCORE_ITEM_S3_BUCKET' is not found."
  );
}
/** 楽譜のスナップショットデータを格納する S3 バケット */
const SCORE_SNAPSHOT_S3_BUCKET = process.env
  .URA_KATA_SCORE_HISTORY_BACKEND_SCORE_SNAPSHOT_S3_BUCKET as string;

if (!SCORE_DYNAMODB_TABLE_NAME) {
  throw new Error(
    "'URA_KATA_SCORE_HISTORY_BACKEND_SCORE_SNAPSHOT_S3_BUCKET' is not found."
  );
}

export class ScoreHistoryBackendStack extends cdk.Stack {
  constructor(scope: cdk.Construct, id: string, props?: cdk.StackProps) {
    super(scope, id, props);

    const scoreDynamoDbTable = new ScoreHistoryBackendScoreDynamoDb(
      this,
      'ScoreHistoryBackendScoreDynamoDb',
      SCORE_DYNAMODB_TABLE_NAME
    );

    const scoreItemDynamoDbTable = new ScoreHistoryBackendScoreItemDynamoDb(
      this,
      'ScoreHistoryBackendScoreItemDynamoDb',
      SCORE_ITEM_DYNAMODB_TABLE_NAME
    );

    const scoreLargeDataDynamoDbTable =
      new ScoreHistoryBackendScoreLargeDataDynamoDb(
        this,
        'ScoreHistoryBackendScoreLargeDataDynamoDb',
        SCORE_LARGE_DATA_DYNAMODB_TABLE_NAME
      );

    const scoreHistoryBackendScoreDataBucket =
      new ScoreHistoryBackendScoreDataBucket(
        this,
        'ScoreHistoryBackendScoreDataBucket',
        SCORE_ITEM_S3_BUCKET
      );

    const scoreHistoryBackendScoreDataSnapshotBucket =
      new ScoreHistoryBackendScoreDataSnapshotBucket(
        this,
        'ScoreHistoryBackendScoreDataSnapshotBucket',
        SCORE_SNAPSHOT_S3_BUCKET
      );
  }
}
