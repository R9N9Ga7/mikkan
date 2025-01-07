type TokenPayload = {
  exp: number,
};

export class Token {
  public constructor(token: string) {
    this.payload = this.parseToken(token);
  }

  public isValid() : boolean {
    const currentTime = new Date().getTime();
    const expTime = new Date(this.payload.exp * 1000).getTime();
    return currentTime < expTime;
  }

  private parseToken (token: string): TokenPayload {
    const [, payloadHash] = token.split('.');
    const payloadString = atob(payloadHash);
    return JSON.parse(payloadString) as TokenPayload;
  };

  private payload: TokenPayload;
}
