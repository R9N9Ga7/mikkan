interface ServerResponse<T> {
  message: string;
  content: T;
}

export default ServerResponse;
